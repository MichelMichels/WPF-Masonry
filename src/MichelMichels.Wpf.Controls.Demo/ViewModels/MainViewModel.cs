using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MichelMichels.Wpf.Controls.Demo.Common;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MichelMichels.Wpf.Controls.Demo.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly Random random = new();
    private readonly RandomBrush randomBrush = new();

    [ObservableProperty]
    private string _amount;

    [ObservableProperty]
    private ObservableCollection<FrameworkElement> _elements = [];

    [ObservableProperty]
    private string _header;

    public MainViewModel()
    {
        _header = "Masonry Example";
        _amount = "5";
    }

    [RelayCommand]
    private void AddClick()
    {
        if (int.TryParse(this.Amount, out int amountValue))
        {
            for (var i = 0; i < amountValue; i++)
            {
                this.Elements.Add(
                    new Border
                    {
                        Width = 200,
                        Height = this.random.Next(100, 300),
                        BorderThickness = new Thickness(1),
                        BorderBrush = Brushes.Black,
                        Background = this.randomBrush.GetRandom(),
                    });
            }
        }
    }

    [RelayCommand]
    private void AddInvisibleElement()
    {
        this.Elements.Add(
            new Border
            {
                Width = 200,
                Height = this.random.Next(100, 300),
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Black,
                Background = this.randomBrush.GetRandom(),
                Visibility = Visibility.Hidden
            }
        );
    }

    [RelayCommand]
    private void ResetClick()
    {
        this.Elements.Clear();
    }

    [RelayCommand]
    private void ChangeRandomElementsVisibility()
    {
        foreach (var element in Elements)
        {
            element.Visibility = random.Next(0, 2) == 0 ? Visibility.Hidden : Visibility.Visible;
        }
    }
}