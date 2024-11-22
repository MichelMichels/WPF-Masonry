using System.Windows;
using System.Windows.Media.Animation;

namespace MichelMichels.Wpf.Controls;

/// <summary>
///     The Animation Item
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="AnimationItem" /> class.
/// </remarks>
/// <param name="element">The element.</param>
/// <param name="animation">The animation.</param>
public class AnimationItem(FrameworkElement element, AnimationTimeline animation)
{

    /// <summary>
    ///     Gets or sets the animation.
    /// </summary>
    /// <value>
    ///     The animation.
    /// </value>
    public AnimationTimeline Animation { get; set; } = animation;

    /// <summary>
    ///     Gets or sets the element.
    /// </summary>
    /// <value>
    ///     The element.
    /// </value>
    public FrameworkElement Element { get; set; } = element;
}