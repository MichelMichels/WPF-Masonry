namespace MichelMichels.Wpf.Controls.Models;

public struct Size(int width, int height) : IEquatable<Size>
{
    public int Width { get; set; } = width;
    public int Height { get; set; } = height;

    public override readonly bool Equals(object? obj)
    {
        return obj is Size other && Equals(other);
    }
    public readonly bool Equals(Size other)
    {
        return other.Width == Width && other.Height == Height;
    }
    public override readonly int GetHashCode()
    {
        return Width.GetHashCode() + Height.GetHashCode();
    }
    public static bool operator ==(Size left, Size right)
    {
        return left.Equals(right);
    }
    public static bool operator !=(Size left, Size right)
    {
        return !(left == right);
    }
}
