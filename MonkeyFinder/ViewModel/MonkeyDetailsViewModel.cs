namespace MonkeyFinder.ViewModel;

[QueryProperty("Monkey","Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    IMap _map;

    public MonkeyDetailsViewModel(IMap map)
    {
        _map = map;
    }
    [ObservableProperty]
    Monkey monkey;

    [RelayCommand]
    async Task OpenMapAsync()
    {
        try
        {
            await _map.OpenAsync(Monkey.Latitude, Monkey.Longitude, new MapLaunchOptions
            {
                Name = Monkey.Name,
                NavigationMode = NavigationMode.None
            });
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
    async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
