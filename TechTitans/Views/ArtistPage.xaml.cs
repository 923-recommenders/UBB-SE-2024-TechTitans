namespace TechTitans.Views;
using TechTitans.Views.Components.Artist;
using TechTitans.Views.Components;
using TechTitans.Models;
using TechTitans.Services;
using Microsoft.Extensions.Configuration;
using System.Data;
using TechTitans.Repositories;

public partial class ArtistPage : ContentPage
{
    public ArtistSongDashboardController Service = ServiceContext.ArtistSongDashboardControllerInstance;
	public ArtistPage()
	{
		InitializeComponent();
        LoadSongs();
	}

    private void LoadSongs()
    {
        var songs = Service.GetSongsByMostPublishedArtistForMainPage();

        SongsGrid.RowDefinitions.Add(new RowDefinition());

        int rowIndex = 0;
        int columnIndex = 0;
        foreach (var song in songs)
        {
            var songItem = new SongItem();
            songItem.BindingContext = song;
            songItem.Margin = new Thickness(0, 5, 0, 5);

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += SongItem_Tapped;
            songItem.GestureRecognizers.Add(tapGestureRecognizer);

            Grid.SetRow(songItem, rowIndex);
            Grid.SetColumn(songItem, columnIndex);
            SongsGrid.Children.Add(songItem);
            columnIndex++;
            if (columnIndex == 2)
            {
                columnIndex = 0;
                rowIndex++;
                SongsGrid.RowDefinitions.Add(new RowDefinition());
            }
        }
    }

    private void SongItem_Tapped(object sender, System.EventArgs e)
    {
        var songItem = (SongItem)sender;
        var songInfo = songItem.BindingContext as SongBasicInformation;
        Navigation.PushAsync(new ArtistSongDashboard(songInfo));
    }

    private List<SongBasicInformation> GetSongs()
    {
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