
using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    private readonly MonkeyService _monkeyService;
    IConnectivity _connectivity;
    IGeolocation _geolocation;

    public ObservableCollection<Monkey> Monkeys { get; } = new ObservableCollection<Monkey>();
    public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
    {
        Title = "Monkey Finder";
        _monkeyService = monkeyService;
        _connectivity = connectivity;
        _geolocation = geolocation;
    }
    

    [RelayCommand]
    async Task GetClosestMonkeyAsync()
    {
        if (IsBusy || !Monkeys.Any()) return;

        try
        {
            IsBusy = true;

            var location = await _geolocation.GetLastKnownLocationAsync();
            if(location is null)
            {
                location = await _geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            }
            if (location == null) return;

            var first = Monkeys.OrderBy(a => location.CalculateDistance(a.Latitude, a.Longitude, DistanceUnits.Kilometers)).FirstOrDefault();
            if (first is null) return;
            await Shell.Current.DisplayAlert("Closest Monkey", $"{first.Name} in {first.Location}", "Ok");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error!", $"Unable to closest monkeys: {ex.Message}", "Ok");

        }
        finally
        {
            IsBusy = false;
        }
    }
    [RelayCommand]
    public async Task GetMonkeysAsync()
    {
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("Internet issue !!", "Check you internet and try again !!", "Ok");
            return;
        }
        if (IsBusy) { return; }
        try
        {
            IsBusy = true;
            var monkeys = await _monkeyService.GetMonkeys();
            if (Monkeys.Any())
            {
                Monkeys.Clear();
            }
            foreach (var monkey in monkeys)
            {
                Monkeys.Add(monkey);
            }

        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error!", $"Unable to get monkeys: {ex.Message}", "Ok");
        }
        finally
        {
            IsBusy = false;
        }
    }
    [RelayCommand]
    public async Task GoToDetailsAsync(Monkey monkey)
    {
        if (monkey is null)
        {
            return;
        }
        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true, new Dictionary<string, object>
        {
            {"Monkey",monkey }
        });
    }
}
