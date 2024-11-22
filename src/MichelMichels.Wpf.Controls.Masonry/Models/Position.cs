namespace MichelMichels.Wpf.Controls.Models;

/// <summary>
/// Place struct for matrix
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="x">Coordinate X</param>
/// <param name="depth">Coordinate Y</param>
/// <param name="width">Width of the element</param>
public struct Position(int x, int depth, int width) : IEquatable<Position>
{

    /// <summary>
    /// Coordinate X
    /// </summary>
    public int X { get; set; } = x;

    /// <summary>
    /// Coordinate Y
    /// </summary>
    public int Depth { get; set; } = depth;

    /// <summary>
    /// Width of the element
    /// </summary>
    public int Width { get; set; } = width;

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
