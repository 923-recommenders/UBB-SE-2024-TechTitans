using TechTitans.Models;
using TechTitans.Services;
using System.IO;
using Microsoft.Extensions.Configuration;


namespace TechTitans.Views;

public partial class AnalystPage : ContentPage
{

	private TopGenresController topGenresController;
	private Dictionary<String, int> genreCount = new Dictionary<String, int>();
	private Dictionary<String, int> subgenreCount = new Dictionary<String, int>();
	public AnalystPage(IConfiguration configuration)
	{
		InitializeComponent();
		topGenresController = new TopGenresController(configuration);
		
	}

	public void ClearText()
	{
        Genre1Name.Text = "";
        Genre1Minutes.Text = "";
        Genre2Name.Text = "";
        Genre2Minutes.Text = "";
        Genre3Name.Text = "";
        Genre3Minutes.Text = "";
    }

	private void OnShowTop3Clicked(object sender, EventArgs e)
	{
		ClearText();
		int monthInt = 0;
		int yearInt = 0;
		var month = MonthPicker.SelectedItem;
		var year = YearPicker.SelectedItem;
		if (month != null && year != null)
		{
			try
			{
				// make them ints
				monthInt = Convert.ToInt32(month);
				yearInt = Convert.ToInt32(year);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			topGenresController.GetTop3Genres(monthInt, yearInt,Genre1Name,Genre1Minutes,Percentage1,Genre2Name,Genre2Name,Percentage2,Genre3Name,Genre3Minutes,Percentage3);
			topGenresController.GetTop3SubGenres(monthInt, yearInt,Subgenre1Name,Subgenre1Minutes,Subgenre1Percentage,Subgenre2Name,Subgenre2Minutes,Subgenre2Percentage,Subgenre3Name,Subgenre3Minutes,Subgenre3Percentage);

		}

    }

}