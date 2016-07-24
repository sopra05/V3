using Microsoft.Xna.Framework;

namespace V3.Widgets
{
    /// <summary>
    /// An element that can be selected.
    /// </summary>
    public interface ISelectable
    {
        bool IsSelected { get; set; }

        /// <summary>
        /// The tooltip to show if this widget is selected.  Can be null or
        /// empty if no tooltip should be shown.
        /// </summary>
        string Tooltip { get; set; }

        /// <summary>
        /// Checks whether this element is selected by a mouse click at the
        /// given position.
        /// </summary>
        /// <param name="position">the position of the mouse click</param>
        /// <returns>true if the element is selected by the mouse click at
        /// that position, otherwise false</returns>
        bool CheckSelected(Point position);
    }
}
