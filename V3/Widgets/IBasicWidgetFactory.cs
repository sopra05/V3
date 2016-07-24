namespace V3.Widgets
{
    /// <summary>
    /// A widget factory that is automatically implemented by Ninject.
    /// </summary>
    public interface IBasicWidgetFactory
    {
        /// <summary>
        /// Creates a new button widget.
        /// </summary>
        /// <returns>a new button widget</returns>
        Button CreateButton();

        /// <summary>
        /// Creates a new empty widget.
        /// </summary>
        /// <returns>a new empty widget</returns>
        EmptyWidget CreateEmptyWidget();

        /// <summary>
        /// Creates a new select button widget.
        /// </summary>
        /// <returns>a new select button widget</returns>
        SelectButton CreateSelectButton();

        /// <summary>
        /// Creates a new label widget.
        /// </summary>
        /// <returns>a new label widget</returns>
        Label CreateLabel();

        /// <summary>
        /// Creates a new Achievement Box.
        /// </summary>
        /// <returns>a new achievement box widget.</returns>
        AchievementBox CreateAchievementBox();
    }
}
