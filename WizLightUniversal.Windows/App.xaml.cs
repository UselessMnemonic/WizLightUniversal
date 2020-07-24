using System;
using System.Windows;
using System.Windows.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;
using Application = Xamarin.Forms.Application;

namespace WizLightUniversal.Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private NotifyIcon trayIcon;
        private System.Drawing.Point lastMousePositionInIcon;
        private volatile bool IsQuitting;

        protected override void OnStartup(StartupEventArgs e)
        {
            Forms.Init();

            base.OnStartup(e);

            // create icon
            trayIcon = new NotifyIcon();
            trayIcon.MouseUp += NotifyIconOnMouseUp;
            trayIcon.MouseMove += NotifyIconOnMouseMove;
            trayIcon.Icon = WizLightUniversal.Windows.Properties.Resources.TrayIcon_32_32;

            // create context menu
            trayIcon.ContextMenuStrip = new ContextMenuStrip();
            trayIcon.ContextMenuStrip.Items.Add("Quit").Click += (s, eArg) => Applicaton_Quit();

            // create window
            MainWindow = new FormsApplicationPage
            {
                Title = "WizLightUniversal",
                Height = 540,
                Width = 400,
                Topmost = true,
                ShowInTaskbar = false,
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None
            };
            ((FormsApplicationPage)MainWindow).LoadApplication(new Core.App());
            MainWindow.Deactivated += MainWindow_Deactivated;

            // enable tray icon and start app
            Core.PreferencesProvider.Default = new WinPreferencesProvider();
            trayIcon.Visible = true;
            IsQuitting = false;
        }

        // Store the current position of the mouse on the icon to check if the mouse clicked inside the icon
        // when the window gets deactivated to avoid a duplicated window toggle
        private void NotifyIconOnMouseMove(object sender, MouseEventArgs e)
        {
            lastMousePositionInIcon = Control.MousePosition;
        }

        // Toggle the window on a left click on the icon
        private void NotifyIconOnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ToggleWindow();
            }
        }

        /// <summary>
        /// Called when clicked outside the window.
        /// Toggles the window to get hidden.
        /// </summary> 
        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            // The window is deactivated anytime it is clicked outside its bounds
            // check that the click didn't happen on the icon itself
            if (lastMousePositionInIcon != Control.MousePosition)
            {
                ToggleWindow();
            }
        }

        private enum QuadrantCorner { TOP_LEFT, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT };

        /// <summary>
        /// Toggles the window between visible and hidden.
        /// </summary>
        private void ToggleWindow()
        {
            if (IsQuitting) return;

            // Hide the window when it is visible
            else if (MainWindow.IsVisible)
            {
                MainWindow.Hide();
                Application.Current.SendSleep();
            }
            // Show the window when it is not visible
            else
            {
                // Position window
                switch (GetTaskbarOrientation())
                {
                    case QuadrantCorner.BOTTOM_RIGHT:
                        MainWindow.Left = SystemParameters.WorkArea.BottomRight.X - MainWindow.Width;
                        MainWindow.Top = SystemParameters.WorkArea.BottomRight.Y - MainWindow.Height;
                        break;
                    case QuadrantCorner.BOTTOM_LEFT:
                        MainWindow.Left = SystemParameters.WorkArea.BottomLeft.X;
                        MainWindow.Top = SystemParameters.WorkArea.BottomLeft.Y - MainWindow.Height;
                        break;
                    case QuadrantCorner.TOP_LEFT:
                        MainWindow.Left = SystemParameters.WorkArea.BottomLeft.X;
                        MainWindow.Top = SystemParameters.WorkArea.BottomLeft.Y;
                        break;
                    case QuadrantCorner.TOP_RIGHT:
                    default:
                        MainWindow.Left = SystemParameters.WorkArea.TopRight.X - MainWindow.Width;
                        MainWindow.Top = SystemParameters.WorkArea.TopRight.Y;
                        break;
                }

                Application.Current.SendResume();
                MainWindow.Show();
                MainWindow.Activate();
            }
        }

        // Get orientation of taskbar, based on the quadrant in which the mouse click resides
        private QuadrantCorner GetTaskbarOrientation()
        {
            // Right side of screen
            if (lastMousePositionInIcon.X > (SystemParameters.WorkArea.Width / 2))
            {
                // Bottom of screen
                if (lastMousePositionInIcon.Y > (SystemParameters.WorkArea.Height / 2))
                {
                    return QuadrantCorner.BOTTOM_RIGHT;
                }

                // Top of screen
                else
                {
                    return QuadrantCorner.TOP_RIGHT;
                }
            }

            // Left side of screen
            else
            {
                // Bottom of screen
                if (lastMousePositionInIcon.Y > (SystemParameters.WorkArea.Height / 2))
                {
                    return QuadrantCorner.BOTTOM_LEFT;
                }

                // Top of screen
                else
                {
                    return QuadrantCorner.TOP_LEFT;
                }
            }
        }

        // Performed when the application must quit
        private void Applicaton_Quit()
        {
            IsQuitting = true;
            trayIcon.Visible = false;
            trayIcon.Dispose();
            trayIcon = null;
            Application.Current.Quit();
            System.Windows.Forms.Application.Exit();
        }
    }
}