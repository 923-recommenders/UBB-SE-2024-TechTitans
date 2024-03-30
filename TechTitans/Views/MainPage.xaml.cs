using TechTitans.Repositories;

namespace TechTitans.Views;

public partial class MainPage : ContentPage
{
    int count = 0;
    public TestRepository Repository { get; set; }

    public MainPage()
    {
        InitializeComponent();
        Repository = new TestRepository();
        var dbData = Repository.TestMethod();
        TestId.Text = dbData.Id.ToString();
        TestName.Text = dbData.Name;

    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}


