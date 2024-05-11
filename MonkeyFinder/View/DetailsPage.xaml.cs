namespace MonkeyFinder;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(MonkeyDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}