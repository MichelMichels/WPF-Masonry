namespace MichelMichels.Wpf.Controls.Models;

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override readonly bool Equals(object? obj)
    {
        return obj is Position other && Equals(other);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override readonly int GetHashCode()
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
    public readonly bool Equals(Position other)
    {
        return other.X == this.X && other.Depth == this.Depth && other.Width == this.Width;
    }
}
