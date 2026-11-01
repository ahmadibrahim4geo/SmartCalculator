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
        private const double FullWindowWidth = 444;
        private const double FullWindowHeight = 854;
        private const double CompactWindowWidth = 380;
        private const double CompactWindowHeight = 540;
        private bool _isCompactMode;
        private Rect _fullWindowBounds;
        private readonly Services.AppSettingsService _settingsService = new();

        public MainWindow()
        {
            InitializeComponent();
            var vm = new CalculatorViewModel();
            DataContext = vm;

            vm.CompactModeChanged += isCompact =>
            {
                if (isCompact)
                    EnterCompactMode();
                else
                    ExitCompactMode();
            };

            // Restore Always-on-Top preference
            Topmost = _settingsService.LoadAlwaysOnTop();
            if (PinButton != null)
            {
                PinButton.Tag = Topmost ? "Pinned" : null;
                PinButton.ToolTip = Topmost ? "Always on Top: On (Click to unpin)" : "Always on Top: Off (Click to pin window)";
            }

            // Restore Compact mode preference
            if (vm.IsCompactMode)
            {
                EnterCompactMode();
            }
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

        private void CompactButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CalculatorViewModel vm)
            {
                vm.ToggleCompactMode();
            }
            else
            {
                if (_isCompactMode)
                    ExitCompactMode();
                else
                    EnterCompactMode();
            }
        }

        private void EnterCompactMode()
        {
            if (!_isCompactMode)
                _fullWindowBounds = new Rect(Left, Top, ActualWidth, ActualHeight);

            _isCompactMode = true;

            FullKeypad.Visibility = Visibility.Collapsed;
            CompactKeypad.Visibility = Visibility.Visible;
            TopControls.Visibility = Visibility.Collapsed;
            FooterDecoration.Visibility = Visibility.Collapsed;

            // BrandSubtitle MUST stay visible in compact mode as requested
            BrandSubtitle.Visibility = Visibility.Visible;
            HeaderToolButtons.Visibility = Visibility.Collapsed;
            DeveloperLogo.Visibility = Visibility.Visible;
            DeveloperLogo.Height = 52;

            HeaderRow.Height = new GridLength(60);
            DisplayRow.Height = new GridLength(110);
            ControlsRow.Height = new GridLength(0);
            FooterRow.Height = new GridLength(0);

            Width = CompactWindowWidth;
            Height = CompactWindowHeight;
            CompactButton.Content = "⤢";
            CompactButton.ToolTip = "Full mode: Restore the scientific calculator (Ctrl+M)";
        }

        private void ExitCompactMode()
        {
            _isCompactMode = false;

            CompactKeypad.Visibility = Visibility.Collapsed;
            FullKeypad.Visibility = Visibility.Visible;
            TopControls.Visibility = Visibility.Visible;
            FooterDecoration.Visibility = Visibility.Visible;

            BrandSubtitle.Visibility = Visibility.Visible;
            HeaderToolButtons.Visibility = Visibility.Visible;
            DeveloperLogo.Visibility = Visibility.Visible;
            DeveloperLogo.Height = 76;

            HeaderRow.Height = new GridLength(82);
            DisplayRow.Height = new GridLength(120);
            ControlsRow.Height = new GridLength(84);
            FooterRow.Height = new GridLength(18);

            Width = _fullWindowBounds.Width > 0 ? _fullWindowBounds.Width : FullWindowWidth;
            Height = _fullWindowBounds.Height > 0 ? _fullWindowBounds.Height : FullWindowHeight;
            CompactButton.Content = "⤡";
            CompactButton.ToolTip = "Compact mode: Show essential calculator controls (Ctrl+M)";
        }

        private void PinButton_Click(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;
            if (PinButton == null) return;
            PinButton.Tag = Topmost ? "Pinned" : null;
            PinButton.ToolTip = Topmost ? "Always on Top: On (Click to unpin)" : "Always on Top: Off (Click to pin window)";
            _settingsService.SaveAlwaysOnTop(Topmost);
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
        private System.Windows.Threading.DispatcherTimer? _skinLongPressTimer;
        private bool _isSkinLongPress;

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

        private void SkinButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isSkinLongPress = false;
            if (_skinLongPressTimer == null)
            {
                _skinLongPressTimer = new System.Windows.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(500)
                };
                _skinLongPressTimer.Tick += (s, ev) =>
                {
                    _skinLongPressTimer.Stop();
                    _isSkinLongPress = true;
                    if (DataContext is CalculatorViewModel vm)
                        vm.ShowSkinsMenu = true;
                };
            }
            _skinLongPressTimer.Start();
        }

        private void SkinButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_skinLongPressTimer != null && _skinLongPressTimer.IsEnabled)
                _skinLongPressTimer.Stop();

            if (!_isSkinLongPress && DataContext is CalculatorViewModel vm &&
                vm.InputCommand.CanExecute("SKIN_CYCLE"))
            {
                vm.InputCommand.Execute("SKIN_CYCLE");
            }
            _isSkinLongPress = false;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var vm = DataContext as CalculatorViewModel;
            if (vm == null) return;

            Key k = e.Key;

            // Global shortcut: Ctrl+M toggles Compact mode
            if (k == Key.M && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                vm.ToggleCompactMode();
                e.Handled = true;
                return;
            }

            string? param = null;

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
                if (vm.ShowThemesMenu || vm.ShowSkinsMenu || vm.ShowConstants || vm.ShowConversions)
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
