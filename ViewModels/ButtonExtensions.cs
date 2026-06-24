using System.Windows;

namespace SmartCalculator.ViewModels
{
    public static class ButtonExtensions
    {
        public static readonly DependencyProperty SecondaryLabelProperty =
            DependencyProperty.RegisterAttached(
                "SecondaryLabel", typeof(string), typeof(ButtonExtensions),
                new PropertyMetadata(null));

        public static void SetSecondaryLabel(DependencyObject element, string value)
            => element.SetValue(SecondaryLabelProperty, value);

        public static string GetSecondaryLabel(DependencyObject element)
            => (string)element.GetValue(SecondaryLabelProperty) ?? "";

        public static readonly DependencyProperty AlphaLabelProperty =
            DependencyProperty.RegisterAttached(
                "AlphaLabel", typeof(string), typeof(ButtonExtensions),
                new PropertyMetadata(null));

        public static void SetAlphaLabel(DependencyObject element, string value)
            => element.SetValue(AlphaLabelProperty, value);

        public static string GetAlphaLabel(DependencyObject element)
            => (string)element.GetValue(AlphaLabelProperty) ?? "";
    }
}
