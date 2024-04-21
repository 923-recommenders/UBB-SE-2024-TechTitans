using System.Globalization;

namespace TechTitans
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            MainPage = new AppShell();
        }
    }
}
