using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;
using Application = Xamarin.Forms.Application;
using Point = Xamarin.Forms.Point;

namespace WizLightUniversal.Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private NotifyIcon _notifyIcon;
        private bool _isExit;

        protected override void OnStartup(StartupEventArgs e)
        {
            Forms.Init();

            base.OnStartup(e);

            _notifyIcon = new NotifyIcon();
            _notifyIcon.MouseUp += NotifyIconOnMouseUp;
            _notifyIcon.MouseMove += NotifyIconOnMouseMove;
            _notifyIcon.Icon = WizLightUniversal.Windows.Properties.Resources.TrayIcon_32_32;
            _notifyIcon.Visible = true;

            CreateContextMenu();
        }

        private void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip =
                new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit",
                WizLightUniversal.Windows.Properties.Resources.TrayIcon_32_32.ToBitmap()).Click +=
                (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            _isExit = true;
            if (MainWindow != null)
            {
                MainWindow.Closing -= MainWindow_Closing;
                //MainWindow.LostFocus -= MainWindow_LostFocus;
                MainWindow.Close();
                MainWindow = null;
            }
            _notifyIcon.Dispose();
            _notifyIcon = null;

            // Stop the application
            Current.Shutdown();
        }

        /// <summary>
        /// Toggles the window between visible and hidden.
        /// </summary>
        private void ToggleWindow()
        {
            // Create window when it is opened for the first time
            if (MainWindow == null)
            {
                MainWindow = new FormsApplicationPage
                {
                    Title = "Xamarin.Forms tray!",
                    Height = 600,
                    Width = 350,
                    Topmost = true,
                    ShowInTaskbar = false,
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.ToolWindow
                };
                ((FormsApplicationPage)MainWindow).LoadApplication(new Core.App());
                MainWindow.Closing += MainWindow_Closing;
                Application.Current.SendStart();
            }

            // Hide the window when it is visible
            if (MainWindow.IsVisible)
            {
                // Hide!
                MainWindow.Deactivated -= MainWindowOnDeactivated;
                MainWindow.Hide();
                Application.Current.SendSleep();
            }
            // Show the window when it is not visible
            else
            {
                // Position window
                MainWindow.Left = SystemParameters.WorkArea.Width - MainWindow.Width - 50;
                MainWindow.Top = SystemParameters.WorkArea.Height - MainWindow.Height - 50;
                // Show!
                MainWindow.Show();
                MainWindow.Activate();
                MainWindow.Deactivated += MainWindowOnDeactivated;
                Application.Current.SendResume();
            }
        }

        private enum TaskBarPosition { Left, Top, Right, Bottom }

        /// <summary>
        /// Get the top left point of the view based on the cursor and task bar position.
        /// </summary>
        private Point GetPosition()
        {
            // The current cursor position (when the icon is pressed)
            var cursor = Control.MousePosition;
            // Additional offset from the taskbar
            var offset = 10;
            // The work area without the task bar where the view is visible
            var desktopWorkingArea = SystemParameters.WorkArea;
            // The complete screen size
            var screenBounds = Screen.PrimaryScreen.Bounds;

            // Get the task bar position
            var taskBarOrientation = TaskBarPosition.Bottom;
            if (desktopWorkingArea.Left > 0)
                taskBarOrientation = TaskBarPosition.Left;
            if (desktopWorkingArea.Right < screenBounds.Right)
                taskBarOrientation = TaskBarPosition.Right;
            if (desktopWorkingArea.Top > 0)
                taskBarOrientation = TaskBarPosition.Top;

            switch (taskBarOrientation)
            {
                case TaskBarPosition.Left:
                    // Window is in the lower left corner
                    return new Point(desktopWorkingArea.Left + offset, cursor.Y - MainWindow.Height);
                case TaskBarPosition.Top:
                    // Window is in the top right corner
                    return new Point(cursor.X - MainWindow.Width, desktopWorkingArea.Top + offset);
                case TaskBarPosition.Right:
                    // Window is in the lower right corner
                    return new Point(desktopWorkingArea.Right - MainWindow.Width - offset, cursor.Y - MainWindow.Height);
                case TaskBarPosition.Bottom:
                    // Window is in the lower right corner
                    return new Point(cursor.X - MainWindow.Width, desktopWorkingArea.Bottom - MainWindow.Height - offset);
                default:
                    return Point.Zero;
            }
        }

        // Toggle the window on a left click on the icon
        private void NotifyIconOnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ToggleWindow();
        }

        // Toggle when the window 'X' was clicked
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                // Only hide the window to avoid recreating it when it should get displayed again
                ToggleWindow();
            }
        }

        private System.Drawing.Point? _lastMousePositionInIcon;

        // Store the current position of the mouse on the icon to check if the mouse clicked inside the icon
        // when the window gets deactivated to avoid a duplicated window toggle
        private void NotifyIconOnMouseMove(object sender, MouseEventArgs e)
        {
            _lastMousePositionInIcon = Control.MousePosition;
        }

        /// <summary>
        /// Called when clicked outside the window.
        /// Toggles the window to get hidden.
        /// </summary> 
        private void MainWindowOnDeactivated(object sender, EventArgs e)
        {
            // Check if the deactivation came by clicking the icon since this already toggles the window
            if (_lastMousePositionInIcon.HasValue && _lastMousePositionInIcon == Control.MousePosition)
                return;
            ToggleWindow();
        }
    }
}