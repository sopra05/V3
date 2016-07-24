namespace V3.Widgets
{
    /// <summary>
    /// A widget factory.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class WidgetFactory
    {
        private IBasicWidgetFactory mFactory;

        public WidgetFactory(IBasicWidgetFactory factory)
        {
            mFactory = factory;
        }

        /// <summary>
        /// Creates a new button with the given text.
        /// </summary>
        /// <param name="text">the text of the button</param>
        /// <returns>the created button</returns>
        public Button CreateButton(string text)
        {
            return CreateTextWidget(mFactory.CreateButton(), text);
        }

        /// <summary>
        /// Creates a new empty widget.
        /// </summary>
        /// <returns>the created widget</returns>
        public EmptyWidget CreateEmptyWidget()
        {
            return mFactory.CreateEmptyWidget();
        }

        /// <summary>
        /// Creates a new select button with the given text options.
        /// </summary>
        /// <param name="values">the values of the button</param>
        /// <returns>the created button</returns>
        public SelectButton CreateSelectButton(string[] values)
        {
            var widget = CreateTextWidget(mFactory.CreateSelectButton(), "");
            foreach (var val in values)
                widget.Values.Add(val);
            return widget;
        }

        /// <summary>
        /// Creates a new select button without options.
        /// </summary>
        /// <returns>the created button</returns>
        public SelectButton CreateSelectButton()
        {
            return mFactory.CreateSelectButton();
        }

        /// <summary>
        /// Creates a new label with the given text.
        /// </summary>
        /// <param name="text">the text of the label</param>
        /// <returns>the created label</returns>
        public Label CreateLabel(string text)
        {
            return CreateTextWidget(mFactory.CreateLabel(), text);
        }

        private T CreateTextWidget<T>(T widget, string text) where T : ITextWidget
        {
            widget.Text = text;
            return widget;
        }

        public AchievementBox CreateAchievementBox()
        {
            return mFactory.CreateAchievementBox();
        }
    }
}
