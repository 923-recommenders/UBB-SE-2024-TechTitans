namespace TechTitans.Views;
using TechTitans.Views.Components.Artist;
using TechTitans.Views.Components;
using TechTitans.Models;
using TechTitans.Services;
using Microsoft.Extensions.Configuration;

public partial class ArtistPage : ContentPage
{
    IConfiguration _configuration;
    public ArtistSongDashboardController service;
	public ArtistPage(IConfiguration configuration)
	{
        _configuration = configuration;
		InitializeComponent();
        service = new ArtistSongDashboardController(configuration);
        LoadSongs();
	}

    private void LoadSongs()
    {
        var songs = service.GetSongsByMostPublishedArtistForMainPage(); // Get your list of songs from somewhere (e.g., database, API, local storage)

        // initial row
        SongsGrid.RowDefinitions.Add(new RowDefinition());


        // Loop through each song and dynamically create SongItem controls
        int rowIndex = 0;
        int columnIndex = 0;
        foreach (var song in songs)
        {
            var songItem = new SongItem(); // Create a new instance of SongItem
            songItem.BindingContext = song; // Set the song as the binding context of the SongItem
            songItem.Margin = new Thickness(0, 5, 0, 5); // Set margin as needed

            // Add TapGestureRecognizer to handle tap event
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += SongItem_Tapped;
            songItem.GestureRecognizers.Add(tapGestureRecognizer);

            // Set the row and column of the SongItem in the grid
            Grid.SetRow(songItem, rowIndex);
            Grid.SetColumn(songItem, columnIndex);
            // Add the SongItem to the grid
            SongsGrid.Children.Add(songItem);
            columnIndex++;
            if (columnIndex == 2) // bun cod
            {
                columnIndex = 0;
                rowIndex++;
                SongsGrid.RowDefinitions.Add(new RowDefinition());
            }
        }
    }

    private void SongItem_Tapped(object sender, System.EventArgs e)
    {
        // open ArtistSongDashboard page with song details
        var songItem = (SongItem)sender;
        var songInfo = songItem.BindingContext as SongBasicInformation;
        Navigation.PushAsync(new ArtistSongDashboard(songInfo,_configuration));
    }

    // Sample method to get list of songs (replace this with your actual method)
    private List<SongBasicInformation> GetSongs()
    {
        // mocked songs, to be replaced with actual data retrieval from db
        return new List<SongBasicInformation>
        {
            new SongBasicInformation
            {
                SongId = 10,
                Name = "Song 1",
                Artist = "Artist 1",
                Image = "song_img_default.png"
            },
            new SongBasicInformation
            {
                SongId = 201,
                Name = "Song 2",
                Artist = "Artist 2",
                Image = "song_img_default.png"
            },
            new SongBasicInformation
            {
                SongId = 2,
                Name = "Song 3",
                Artist = "Artist 3",
                Image = "song_img_default.png"
            },
            new SongBasicInformation
            {
                SongId = 3,
                Name = "Song 4",
                Artist = "Artist 4",
                Image = "song_img_default.png"
            },
            new SongBasicInformation
            {
                SongId = 4,
                Name = "Song 5",
                Artist = "Artist 5",
                Image = "song_img_default.png"
            },
            new SongBasicInformation
            {
                SongId = 5,
                Name = "Song 6",
                Artist = "Artist 6",
                Image = "song_img_default.png"
            },
            new SongBasicInformation
            {
                SongId = 6,
                Name = "Song 7",
                Artist = "Artist 7",
                Image = "song_img_default.png"
            },
            new SongBasicInformation
            {
                SongId = 7,
                Name = "Song 8",
                Artist = "Artist 8",
                Image = "song_img_default.png"
            },
            new SongBasicInformation
            {
                SongId = 8,
                Name = "Song 9",
                Artist = "Artist 9",
                Image = "song_img_default.png"
            },
        };
    }

}