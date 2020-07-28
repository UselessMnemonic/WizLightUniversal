using System;
using AppKit;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;
using CoreGraphics;

namespace WizLightUniversal.macOS
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        private NSStatusItem statusItem;
        private NSMenu statusMenu;
        private NSPopover popover;

        // constructor
        public AppDelegate()
        {
            // Init Xamarin.Forms
            Forms.Init();

            // Create the tray item w/ icon and menu
            statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(NSStatusItemLength.Square);
            statusItem.Button.Image = NSImage.ImageNamed("TrayIcon_White.ico").ResizeTo(new CGSize(18, 18));

            statusMenu = new NSMenu();
            NSMenuItem quitMenuItem = new NSMenuItem("Quit");
            quitMenuItem.Activated += (object sender, EventArgs e) => { NSApplication.SharedApplication.Terminate(this); };
            statusMenu.AddItem(quitMenuItem);

            // The core app will appear inside a popover
            Core.PreferencesProvider.Default = new MacPreferencesProvider();
            popover = new NSPopover();
            Application.SetCurrentApplication(new Core.App());
        }

        // Called after the application is ready to open the GUI
        public override void DidFinishLaunching(NSNotification notification)
        {
            // Create container for popover
            Container container = Container.FreshController();

            // Furnish popover with content
            popover.ContentViewController = container;
            popover.Behavior = NSPopoverBehavior.Transient;
            popover.Delegate = new PopoverDelegate();
            popover.Animates = false;
            popover.SetAppearance(NSAppearance.GetAppearance(NSAppearance.NameLightContent));

            // Enable the tray item only after the app has launched
            statusItem.Button.Activated += StatusItem_Click;
            statusItem.Button.SendActionOn(NSEventType.OtherMouseUp);
            statusItem.Visible = true;
            Application.Current.SendStart();
        }

        // Called when the user quits
        public override void WillTerminate(NSNotification notification)
        {
            // Teardown resources
            statusItem.Visible = false;
            statusItem.Dispose();

            statusMenu.Dispose();

            if (popover.Shown) popover.Close();
            popover.Dispose();

            Application.Current.Quit();
        }

        // Called when the tray icon is clicked
        private void StatusItem_Click(object sender, EventArgs e)
        {
            switch (NSApplication.SharedApplication.CurrentEvent.Type)
            {
                // When the tray item gets a left-click, toggle the popover
                case NSEventType.LeftMouseDown:
                    if (!popover.Shown)
                    {
                        popover.Show(statusItem.Button.Bounds, statusItem.Button, NSRectEdge.MaxYEdge);
                    }
                    else
                    {
                        popover.Close();
                    }
                    break;

                // When the tray item gits a right-click, show the menu
                case NSEventType.RightMouseDown:
                    statusItem.PopUpStatusItemMenu(statusMenu);
                    break;
            }
        }
    }

    // A class that responds to events issued by the popover
    public class PopoverDelegate : NSPopoverDelegate
    {
        private readonly object appLock;

        public PopoverDelegate()
        {
            appLock = new object();
        }

        // Send a sleep signal to the Xamarin app when the popover is hidden
        public override void DidClose(NSNotification notification)
        {
            lock (appLock)
            {
                Application.Current.SendSleep();
            }
        }

        // Send a wake signal to the Xamaran app when the popover is shown
        public override void DidShow(NSNotification notification)
        {
            lock (appLock)
            {
                Application.Current.SendResume();
            }
        }
    }
}
