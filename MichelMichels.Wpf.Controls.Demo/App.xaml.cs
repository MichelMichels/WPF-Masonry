using MichelMichels.Wpf.Controls.Demo.ViewModels;
using MichelMichels.Wpf.Controls.Demo.Views;
using System.Windows;

namespace MichelMichels.Wpf.Controls.Demo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    protected override void OnStartup(StartupEventArgs e)
    {
        MainView view = new()
        {
            DataContext = new MainViewModel(),
        };

        view.ShowDialog();
    }
}
