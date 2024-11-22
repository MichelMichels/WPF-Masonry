using System.Windows;
using System.Windows.Media.Animation;

namespace MichelMichels.Wpf.Controls;

/// <summary>
///     The Animation Manager
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="AnimationManager" /> class.
/// </remarks>
/// <param name="animationProperty">The animation property.</param>
public class AnimationManager(DependencyProperty animationProperty)
{
    /// <summary>
    ///     The active animations
    /// </summary>
    private readonly List<AnimationItem> activeAnimations = [];

    /// <summary>
    ///     The animation property
    /// </summary>
    private readonly DependencyProperty animationProperty = animationProperty;

    /// <summary>
    ///     The animation queue
    /// </summary>
    private readonly Queue<AnimationItem> animationQueue = new();

    /// <summary>
    ///     Occurs when [on completed].
    /// </summary>
    public event EventHandler? OnCompleted;

    /// <summary>
    ///     Gets a value indicating whether an animation is running.
    /// </summary>
    /// <value>
    ///     <c>true</c> if an animation is running; otherwise, <c>false</c>.
    /// </value>
    public bool IsRunning => this.activeAnimations.Count != 0;

    /// <summary>
    ///     Enqueues the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="animation">The animation.</param>
    public void Enqueue(FrameworkElement element, AnimationTimeline animation)
    {
        if (element != null && animation != null)
        {
            this.animationQueue.Enqueue(new AnimationItem(element, animation));
        }
    }

    /// <summary>
    ///     Starts the animation.
    /// </summary>
    public void Start()
    {
        while (this.animationQueue.Count != 0)
        {
            var animationItem = this.animationQueue.Dequeue();
            if (animationItem != null && animationItem.Element.IsLoaded)
            {
                this.ClearActiveAnimations(animationItem);
                this.HandleAnimationEvent(animationItem);
                animationItem.Element.BeginAnimation(this.animationProperty, animationItem.Animation);
            }
        }
    }

    /// <summary>
    ///     Clears the active animations.
    /// </summary>
    /// <param name="animationItem">The animation item.</param>
    protected void ClearActiveAnimations(AnimationItem animationItem)
    {
        ArgumentNullException.ThrowIfNull(animationItem);

        foreach (var activeAnimation in this.activeAnimations.ToArray())
        {
            if (activeAnimation.Element.Equals(animationItem.Element))
            {
                this.activeAnimations.Remove(activeAnimation);
            }
        }
    }

    /// <summary>
    ///     Handles the animation event.
    /// </summary>
    /// <param name="animationItem">The animation item.</param>
    protected void HandleAnimationEvent(AnimationItem animationItem)
    {
        ArgumentNullException.ThrowIfNull(animationItem);

        foreach (var a in this.activeAnimations.ToArray())
        {
            if (a.Element.Equals(animationItem.Element))
            {
                this.activeAnimations.Remove(a);
            }
        }
        this.activeAnimations.Add(animationItem);
        animationItem.Animation.Completed += delegate
            {
                this.activeAnimations.Remove(animationItem);
                if (this.activeAnimations.Count == 0)
                {
                    this.OnCompleted?.Invoke(null, EventArgs.Empty);
                }
            };
    }
}

