using System.Reflection;
using System.Windows.Media;


namespace MichelMichels.Wpf.Controls.Demo.Common;

public class RandomBrush
{
    private readonly List<Brush> brushes;

    private readonly Random random;

    public RandomBrush()
    {
        this.random = new Random();
        this.brushes = new List<Brush>();
        var props = typeof(Brushes).GetProperties(BindingFlags.Public | BindingFlags.Static);
        foreach (var propInfo in props)
        {
            if (!propInfo.Name.Contains("White") && !propInfo.Name.Contains("Gray"))
            {
                this.brushes.Add((Brush)propInfo.GetValue(null, null));
            }
        }
    }

    public Brush GetRandom()
    {
        return this.brushes[this.random.Next(this.brushes.Count)];
    }
}