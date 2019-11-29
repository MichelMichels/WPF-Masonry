#region License

/*
 Copyright 2013 - 2016 Nikita Bernthaler
 Copyright 2019 - 2019 Michel Michels
 Position.cs is part of Masonry.

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masonry.Models
{
    /// <summary>
    /// Place struct for matrix
    /// </summary>
    public struct Position : IEquatable<Position>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Coordinate X</param>
        /// <param name="depth">Coordinate Y</param>
        /// <param name="width">Width of the element</param>
        public Position(int x, int depth, int width)
        {
            X = x;
            Depth = depth;
            Width = width;
        }

        #region Properties
        /// <summary>
        /// Coordinate X
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Coordinate Y
        /// </summary>
        public int Depth { get; set; }
        
        /// <summary>
        /// Width of the element
        /// </summary>
        public int Width { get; set; }
        #endregion

        #region Overrides
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is Position other ? Equals(other) : false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Depth.GetHashCode() + Width.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Equals override
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Position other)
        {
            return other.X == this.X && other.Depth == this.Depth && other.Width == this.Width;
        }
        #endregion
    }
}
