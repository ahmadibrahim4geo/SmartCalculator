using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using SmartCalculator.ViewModels;

namespace SmartCalculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new CalculatorViewModel();
        }

        private void HeaderDragArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState != MouseButtonState.Pressed)
                return;

            if (IsInteractiveElement(e.OriginalSource as DependencyObject))
                return;

            try
            {
                DragMove();
            }
            catch
            {
                // Ignore DragMove exceptions safely
            }
        }

        private static bool IsInteractiveElement(DependencyObject? source)
        {
            while (source != null)
            {
                if (source is Button or ToggleButton or TextBox or ComboBox or
                    Slider or ListBox or MenuItem or ScrollBar)
                {
                    return true;
                }
                source = VisualTreeHelper.GetParent(source);
            }
            return false;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PinButton_Click(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;
            if (PinButton == null) return;

            if (Topmost)
            {
                PinButton.Content = "\U0001F4CD";
                PinButton.Foreground = Brushes.White;
                PinButton.Background = new SolidColorBrush(Color.FromArgb(0x30, 0xFF, 0xFF, 0xFF));
            }
            else
            {
                PinButton.Content = "\U0001F4CC";
                PinButton.Foreground = new SolidColorBrush(Color.FromRgb(0xC9, 0xCD, 0xD2));
                PinButton.Background = new SolidColorBrush(Color.FromArgb(0x0D, 0xFF, 0xFF, 0xFF));
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem != null)
            {
                listBox.ScrollIntoView(listBox.SelectedItem);
            }
        }

        private System.Windows.Threading.DispatcherTimer? _longPressTimer;
        private bool _isLongPress = false;

        private void ThemeButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isLongPress = false;
            if (_longPressTimer == null)
            {
                _longPressTimer = new System.Windows.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(500)
                };
                _longPressTimer.Tick += (s, ev) =>
                {
                    _longPressTimer.Stop();
                    _isLongPress = true;
                    var vm = DataContext as CalculatorViewModel;
                    if (vm != null)
                    {
                        vm.ShowThemesMenu = true;
                    }
                };
            }
            _longPressTimer.Start();
        }

        private void ThemeButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_longPressTimer != null && _longPressTimer.IsEnabled)
            {
                _longPressTimer.Stop();
            }

            if (!_isLongPress)
            {
                var vm = DataContext as CalculatorViewModel;
                if (vm != null && vm.InputCommand.CanExecute("THEME_CYCLE"))
                {
                    vm.InputCommand.Execute("THEME_CYCLE");
                }
            }
            _isLongPress = false;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var vm = DataContext as CalculatorViewModel;
            if (vm == null) return;

            string? param = null;
            Key k = e.Key;

            if (k == Key.D0 || k == Key.NumPad0) param = "0";
            else if (k == Key.D1 || k == Key.NumPad1) param = "1";
            else if (k == Key.D2 || k == Key.NumPad2) param = "2";
            else if (k == Key.D3 || k == Key.NumPad3) param = "3";
            else if (k == Key.D4 || k == Key.NumPad4) param = "4";
            else if (k == Key.D5 || k == Key.NumPad5) param = "5";
            else if (k == Key.D6 || k == Key.NumPad6) param = "6";
            else if (k == Key.D7 || k == Key.NumPad7) param = "7";
            else if (k == Key.D8 || k == Key.NumPad8) param = "8";
            else if (k == Key.D9 || k == Key.NumPad9) param = "9";
            else if (k == Key.Add) param = "+";
            else if (k == Key.Subtract) param = "-";
            else if (k == Key.Multiply) param = "×";
            else if (k == Key.Divide) param = "÷";
            else if (k == Key.Decimal || k == Key.OemPeriod) param = ".";
            else if (k == Key.OemComma) param = ",";
            else if (k == Key.OemPlus && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) param = "+";
            else if (k == Key.OemPlus && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift) param = "=";
            else if (k == Key.OemMinus) param = "-";
            else if (k == Key.OemOpenBrackets && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift) param = "[";
            else if (k == Key.OemOpenBrackets && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) param = "(";
            else if (k == Key.OemCloseBrackets && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift) param = "]";
            else if (k == Key.OemCloseBrackets && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) param = ")";
            else if (k == Key.Enter || k == Key.Return)
            {
                if (vm.ShowThemesMenu || vm.ShowConstants || vm.ShowConversions)
                {
                    param = "NAV";
                    DpadNav.Tag = "KeyboardPressed";
                }
                else
                {
                    param = "=";
                }
            }
            else if (k == Key.Back) param = "DEL";
            else if (k == Key.Escape) param = "AC";
            else if (k == Key.Up) { param = "UP"; DpadUp.Tag = "KeyboardPressed"; }
            else if (k == Key.Down) { param = "DOWN"; DpadDown.Tag = "KeyboardPressed"; }
            else if (k == Key.Left) { param = "LEFT"; DpadLeft.Tag = "KeyboardPressed"; }
            else if (k == Key.Right) { param = "RIGHT"; DpadRight.Tag = "KeyboardPressed"; }

            // Letter keys for HEX input in BASE-N mode (A-F) or ALPHA variable entry
            if (param == null)
            {
                char? letter = k switch
                {
                    Key.A => 'A', Key.B => 'B', Key.C => 'C', Key.D => 'D',
                    Key.E => 'E', Key.F => 'F', Key.X => 'X', Key.Y => 'Y',
                    Key.M => 'M', Key.I => 'i', _ => null
                };
                if (letter.HasValue)
                {
                    if (vm.IsBaseNActive)
                        param = letter.Value.ToString();
                    else if (vm.IsCmplxActive && letter.Value == 'i')
                        param = letter.Value.ToString();
                    else if (vm.IsAlphaActive)
                        param = letter.Value.ToString();
                }
            }

            if (param != null)
            {
                if (vm.InputCommand.CanExecute(param))
                    vm.InputCommand.Execute(param);
                e.Handled = true;
            }
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            Key k = e.Key;
            if (k == Key.Up) DpadUp.Tag = null;
            else if (k == Key.Down) DpadDown.Tag = null;
            else if (k == Key.Left) DpadLeft.Tag = null;
            else if (k == Key.Right) DpadRight.Tag = null;
            else if (k == Key.Enter || k == Key.Return) DpadNav.Tag = null;
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string chmPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Help.chm");
                if (System.IO.File.Exists(chmPath))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(chmPath) { UseShellExecute = true });
                }
                else
                {
                    string fallbackPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Help.html");
                    if (System.IO.File.Exists(fallbackPath))
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(fallbackPath) { UseShellExecute = true });
                    }
                    else
                    {
                        MessageBox.Show("Help file (Help.chm or Help.html) not found in the application directory.", "Help Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open help file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
