using System.Reflection;
using System.Windows.Media;


namespace MichelMichels.Wpf.Controls.Masonry.Demo.Common;

public class RandomBrush
{
    private readonly List<Brush> brushes;

    private readonly Random random;

    public RandomBrush()
    {
        random = new Random();
        brushes = [];
        var props = typeof(Brushes).GetProperties(BindingFlags.Public | BindingFlags.Static);
        foreach (var propInfo in props)
        {
            if (!propInfo.Name.Contains("White") && !propInfo.Name.Contains("Gray"))
            {
                if (propInfo.GetValue(null, null) is not Brush brush)
                {
                    continue;
                }

                brushes.Add(brush);
            }
        }
    }

    public Brush GetRandom()
    {
        return brushes[random.Next(brushes.Count)];
    }
}