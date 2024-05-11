
using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    private readonly MonkeyService _monkeyService;
    public ObservableCollection<Monkey> Monkeys { get; } = new ObservableCollection<Monkey>();
    public MonkeysViewModel(MonkeyService monkeyService)
    {
        Title = "Monkey Finder";
        _monkeyService = monkeyService;
    }
    [RelayCommand]
    public async Task GetMonkeysAsync()
    {
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
}
