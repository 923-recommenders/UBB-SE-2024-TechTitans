using TechTitans.Models;
using TechTitans.Repositories;

namespace TechTitans.Views;

public partial class MainPage : ContentPage
{
    private int count = 0;
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnUserCLicked(object sender, EventArgs e) => Navigation.PushAsync(new UserPage());
    private void OnArtistClicked(object sender, EventArgs e) => Navigation.PushAsync(new ArtistPage());
    private void OnAnalystClicked(object sender, EventArgs e) => Navigation.PushAsync(new AnalystPage());
    private void OnEndOfYearRecapClicked(object sender, EventArgs e) => Navigation.PushAsync(new EndOfYearRecap());
}