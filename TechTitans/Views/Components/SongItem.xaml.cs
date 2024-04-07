using TechTitans.Models;

namespace TechTitans.Views.Components;

public partial class SongItem : ContentView
{
    public SongBasicInfo mockSong = new()
    {
        SongId = 1,
        Name = "Roma",
        Genre = "Manele",
        Subgenre = "Trapanele",
        Artist = "BDLP",
        Features = new[] { "Ian" },
        Language = "Romanian",
        Country = "Romania",
        Album = "Single",
        Image = "https://i.ytimg.com/vi/Ovbn5mPit8o/sddefault.jpg?v=64c3f573"
    };

    public SongItem()
	{
        if (BindingContext == null) 
        {
            BindingContext = this.mockSong;
        }
		InitializeComponent();
	}
}