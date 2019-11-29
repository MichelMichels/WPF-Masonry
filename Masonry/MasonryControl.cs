#region License

/*
 Copyright 2013 - 2016 Nikita Bernthaler
 Copyright 2019 Michel Michels
 MasonryControl.cs is part of Masonry.

 Masonry is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 Masonry is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with Masonry. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion License

namespace Masonry
{
    using Masonry.Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    ///     The Masonry Control
    /// </summary>
    /// <seealso cref="ItemsControl" />
    public class MasonryControl : ItemsControl
    {
        #region Static Fields

        /// <summary>
        ///     The spacing property
        /// </summary>
        public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(
            "Spacing",
            typeof(int),
            typeof(MasonryControl));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MasonryControl" /> class.
        /// </summary>
        public MasonryControl()
        {
            this.ItemsPanel = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(Grid)));
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the spacing.
        /// </summary>
        /// <value>
        ///     The spacing.
        /// </value>
        public int Spacing
        {
            get
            {
                return (int)this.GetValue(SpacingProperty);
            }
            set
            {
                this.SetValue(SpacingProperty, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Updates this instance.
        /// </summary>
        public virtual void Update()
        {
            var matrix = new List<Position>
            {
                new Position(0, 0, (int)ActualWidth),
            };

            var maxHeight = 0;
            foreach (var child in this.Items)
            {
                if (child is FrameworkElement element)
                {
                    var size = new Models.Size((int)element.ActualWidth + this.Spacing, (int)element.ActualHeight + this.Spacing);                   
                    var point = GetAttachPoint(matrix, size.Width);

                    matrix = UpdateAttachArea(matrix, point, size);
                    maxHeight = Math.Max(maxHeight, (int)point.Y + size.Height);
                    this.UpdateAlignment(element);
                    var oldThickness = element.Margin;
                    if (Math.Abs(oldThickness.Left - (int)point.X) > 1 || Math.Abs(oldThickness.Top - (int)point.Y) > 1)
                    {
                        this.SetPosition(element, (int)point.Y, (int)point.X);
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds the specified object as the child of the <see cref="System.Windows.Controls.ItemsControl" /> object.
        /// </summary>
        /// <param name="value">The object to add as a child.</param>
        /// <exception cref="InvalidDataException">Child has to derive from FrameworkElement.</exception>
        protected override void AddChild(object value)
        {
            if (!(value is FrameworkElement))
            {
                throw new InvalidDataException("Child has to derive from FrameworkElement.");
            }

            base.AddChild(value);
        }

        /// <summary>
        ///     Handles the child desired size changed.
        /// </summary>
        /// <param name="child">The child.</param>
        protected virtual void HandleChildDesiredSizeChanged(UIElement child)
        {
            this.HandleUpdate(child as FrameworkElement);
        }

        /// <summary>
        ///     Handles the render size changed.
        /// </summary>
        /// <param name="sizeInfo">The size information.</param>
        protected virtual void HandleRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            this.Update();
        }

        /// <summary>
        ///     Handles the update.
        /// </summary>
        /// <param name="element">The element.</param>
        protected void HandleUpdate(FrameworkElement element)
        {
            if (element != null)
            {
                if (element.IsLoaded)
                {
                    this.Update();
                }
                else
                {
                    element.Loaded += delegate { this.Update(); };
                }
            }
        }

        /// <summary>
        ///     Supports layout behavior when a child element is resized.
        /// </summary>
        /// <param name="child">The child element that is being resized.</param>
        protected override void OnChildDesiredSizeChanged(UIElement child)
        {
            base.OnChildDesiredSizeChanged(child);
            this.HandleChildDesiredSizeChanged(child);
        }

        /// <summary>
        ///     Invoked when the <see cref="ItemsControl.Items" /> property changes.
        /// </summary>
        /// <param name="e">Information about the change.</param>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if(e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.NewItems != null)
            {
                foreach (var child in e.NewItems)
                {
                    this.HandleUpdate(child as FrameworkElement);
                }
            }
        }

        /// <summary>
        ///     Called when the <see cref="ItemsControl.ItemsSource" /> property changes.
        /// </summary>
        /// <param name="oldValue">Old value of the <see cref="ItemsControl.ItemsSource" /> property.</param>
        /// <param name="newValue">New value of the <see cref="ItemsControl.ItemsSource" /> property.</param>
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            var newValueArray = newValue as object[] ?? newValue.Cast<object>().ToArray();
            base.OnItemsSourceChanged(oldValue, newValueArray);
            if (newValue != null)
            {
                foreach (var child in newValueArray)
                {
                    this.HandleUpdate(child as FrameworkElement);
                }
            }
        }

        /// <summary>
        ///     Raises the <see cref="FrameworkElement.SizeChanged" /> event, using the specified information as
        ///     part of the eventual event data.
        /// </summary>
        /// <param name="sizeInfo">Details of the old and new size involved in the change.</param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            this.HandleRenderSizeChanged(sizeInfo);
        }

        /// <summary>
        ///     Sets the position.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="newTop">The new top.</param>
        /// <param name="newLeft">The new left.</param>
        protected virtual void SetPosition(FrameworkElement element, int newTop, int newLeft)
        {
            if (element != null)
            {
                element.Margin = new Thickness(newLeft, newTop, 0, 0);
            }
        }

        /// <summary>
        ///     Updates the alignment.
        /// </summary>
        /// <param name="element">The element.</param>
        protected virtual void UpdateAlignment(FrameworkElement element)
        {
            if (element != null)
            {
                element.HorizontalAlignment = HorizontalAlignment.Left;
                element.VerticalAlignment = VerticalAlignment.Top;
            }
        }

        /// <summary>
        ///     Gets the attach point.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="width">The width.</param>
        /// <returns></returns>
        private Point GetAttachPoint(List<Position> matrix, int width)
        {
            matrix.Sort(this.MatrixSortDepth);

            var maxWidth = matrix.Last().Width;
            foreach(var position in matrix)
            {
                if (position.Depth >= maxWidth)
                {
                    break;
                }
                if (position.Width - position.X >= width)
                {
                    return new Point(position.X, position.Depth);
                }
            }

            return new Point(0, maxWidth);
        }

        /// <summary>
        ///     Matrixes the join.
        /// </summary>
        /// <param name="matrix">The MTX.</param>
        /// <param name="cell">The cell.</param>
        /// <returns></returns>
        private List<Position> MatrixJoin(List<Position> matrix, Position cell)
        {
            matrix.Add(cell);
            matrix.Sort(this.MatrixSortX);

            var joinedMatrix = new List<Position>();
            foreach(var position in matrix)
            {
                if (joinedMatrix.Any() && joinedMatrix.Last().Width == position.X && joinedMatrix.Last().Depth == position.Depth)
                {
                    var element = joinedMatrix.Last();
                    element.Width = position.Width;            
                }
                else
                {
                    joinedMatrix.Add(position);
                }
            }
            
            return joinedMatrix;
        }

        /// <summary>
        ///     Matrixes the sort depth.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        private int MatrixSortDepth(Position a, Position b)
        {
            return (a.Depth == b.Depth && a.X > b.X) || a.Depth > b.Depth ? 1 : -1;
        }

        /// <summary>
        ///     Matrixes the sort x.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        private int MatrixSortX(Position a, Position b)
        {
            return a.X > b.X ? 1 : -1;
        }

        /// <summary>
        ///     Matrixes the width of the trim.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        private Position MatrixTrimWidth(Position a, Position b)
        {
            if (a.X >= b.X && a.X < b.Width || a.Width >= b.X && a.Width < b.Width)
            {
                if (a.X >= b.X && a.X < b.Width)
                {
                    a.X = b.Width;
                }
                else
                {
                    a.Width = b.X;
                }
            }
            return a;
        }

        /// <summary>
        ///     Updates the attach area.
        /// </summary>
        /// <param name="matrix">The MTX.</param>
        /// <param name="point">The point.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        private List<Position> UpdateAttachArea(List<Position> matrix, Point point, Models.Size size)
        {
            matrix.Sort(this.MatrixSortDepth);
            var cell = new Position((int)point.X, (int)point.Y + size.Height, (int)point.X + size.Width);
                    
            for (int i = 0, length = matrix.Count; i < length; i++)
            {
                if (matrix.Count - 1 >= i)
                {
                    if (cell.X <= matrix[i].X && matrix[i].Width <= cell.Width)
                    {
                        matrix.RemoveAt(i);
                    }
                    else
                    {
                        matrix[i] = this.MatrixTrimWidth(matrix[i], cell);
                    }
                }
            }
            return MatrixJoin(matrix, cell);
        }

        #endregion
    }
}