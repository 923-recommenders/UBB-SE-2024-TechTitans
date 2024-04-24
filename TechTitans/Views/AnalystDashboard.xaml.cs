using TechTitans.Models;
using TechTitans.Services;
// using System.IO;
namespace TechTitans.Views;
using TechTitans.Repositories;
using Microsoft.Extensions.Configuration;
using System.Data;

public partial class AnalystDashboard : ContentPage
{
    public FullDetailsOnSongController FullDetailsController = ServiceContext.FullDetailsOnSongController;
    public AnalystDashboard()
	{
		InitializeComponent();
        FullDetailsOnSong fullDetails = FullDetailsController.GetFullDetailsOnSong(201);
        FullDetailsOnSong currentMonth = FullDetailsController.GetCurrentMonthDetails(201);
        this.BindingContext = currentMonth;
    }
}