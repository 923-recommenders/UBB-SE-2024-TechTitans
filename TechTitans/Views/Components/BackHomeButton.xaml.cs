using Microsoft.Extensions.Configuration;

namespace TechTitans.Views.Components;

public partial class BackHomeButton : ContentView
{
	IConfiguration _configuration;
	public BackHomeButton(IConfiguration configuration)
	{
		_configuration = configuration;
        InitializeComponent();
	}

	private void OnBackClick(object sender, EventArgs e) => Application.Current.MainPage = new NavigationPage(new MainPage(_configuration));

}