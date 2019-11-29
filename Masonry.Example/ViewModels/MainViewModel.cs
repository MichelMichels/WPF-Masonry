#region License

/*
 Copyright 2013 - 2016 Nikita Bernthaler
 MainViewModel.cs is part of Masonry.Example.

 Masonry.Example is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 Masonry.Example is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with Masonry.Example. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion License

namespace Masonry.Example.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Masonry.Example.Common;

    using Prism.Commands;
    using Prism.Mvvm;

    internal class MainViewModel : BindableBase
    {
        #region Fields
        private readonly Random random;
        private readonly RandomBrush randomBrush;
        private string amount;
        private ObservableCollection<FrameworkElement> elements;
        private string header;
        #endregion

        #region Constructors and Destructors
        public MainViewModel()
        {
            this.random = new Random();
            this.randomBrush = new RandomBrush();
            this.Elements = new ObservableCollection<FrameworkElement>();
            this.Header = "Masonry Example";
            this.Amount = "5";
        }
        #endregion

        #region Public Properties
        public ICommand AddClickCommand => new DelegateCommand(this.OnAddButtonClick);
        public ICommand ResetClickCommand => new DelegateCommand(this.OnResetButtonClick);
        public ICommand AddInvisibleElementCommand => new DelegateCommand(this.OnAddInvisibleElement);
        public ICommand ChangeRandomElementsVisibilityCommand => new DelegateCommand(this.ChangeRandomElementsVisibility);

        public string Amount
        {
            get => this.amount;
            set => SetProperty(ref this.amount, value);            
        }
        public ObservableCollection<FrameworkElement> Elements
        {
            get => this.elements;
            set => SetProperty(ref this.elements, value);            
        }
        public string Header
        {
            get => this.header;
            set => SetProperty(ref this.header, value);            
        }
        #endregion

        #region Methods
        private void OnAddButtonClick()
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
        private void OnAddInvisibleElement()
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
        private void OnResetButtonClick()
        {
            this.Elements.Clear();
        }
        private void ChangeRandomElementsVisibility()
        {
            foreach(var element in Elements)
            {
                element.Visibility = random.Next(0, 2) == 0 ? Visibility.Hidden : Visibility.Visible;
            }
        }

        #endregion
    }
}