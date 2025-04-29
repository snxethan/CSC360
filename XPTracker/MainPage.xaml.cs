using XPTracker.ViewModels;

namespace XPTracker;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCustomXPEntered(object sender, EventArgs e)
    {
        if (sender is Entry entry && int.TryParse(entry.Text, out int xp) && entry.BindingContext is Player player)
        {
            ((XPTrackerViewModel)BindingContext).ModifyXPCommand.Execute((player, xp));
            entry.Text = string.Empty; // Clear input after processing
        }
    }

    private void On25XPClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Player player)
        {
            ((XPTrackerViewModel)BindingContext).ModifyXPCommand.Execute((player, 25));
        }
    }

    private void On50XPClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Player player)
        {
            ((XPTrackerViewModel)BindingContext).ModifyXPCommand.Execute((player, 50));
        }
    }


    private void On100XPClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Player player)
        {
            ((XPTrackerViewModel)BindingContext).ModifyXPCommand.Execute((player, 100));
        }
    }

    private void On500XPClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Player player)
        {
            ((XPTrackerViewModel)BindingContext).ModifyXPCommand.Execute((player, 500));
        }
    }



    // CLOSE APP
    private async void OnCloseClicked(object sender, EventArgs e)
    {
        var exit = await DisplayAlert("Exit", "Do you want to close the application?", "Yes", "No");
        if (exit)
            System.Diagnostics.Process.GetCurrentProcess().Kill();
    }
}