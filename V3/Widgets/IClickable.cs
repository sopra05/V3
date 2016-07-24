using V3.Input;

namespace V3.Widgets
{
    /// <summary>
    /// An element that can be clicked on.
    /// </summary>
    public interface IClickable : IMouseEventHandler
    {
        bool IsClicked { get; set; }

        bool IsEnabled { get; set; }
    }
}
