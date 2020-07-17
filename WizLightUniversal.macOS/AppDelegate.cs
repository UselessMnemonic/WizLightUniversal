using System;
using AppKit;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace WizLightUniversal.macOS
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        private NSStatusItem statusItem;
        private NSMenu statusMenu;
        private NSPopover popover;

        public AppDelegate()
        {
            // Init Xamarin.Forms
            Forms.Init();

            // Create the tray item w/ icon and menu
            statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(NSStatusItemLength.Square);
            statusItem.Button.Image = NSImage.ImageNamed("TrayIcon_18_18.ico");

            statusMenu = new NSMenu();
            NSMenuItem quitMenuItem = new NSMenuItem("Quit");
            quitMenuItem.Activated += (object sender, EventArgs e) => { NSApplication.SharedApplication.Terminate(this); };
            statusMenu.AddItem(quitMenuItem);

            // The core app will run under this app delegate, using the Mac preferences provider
            Core.PreferencesProvider.Default = new MacPreferencesProvider();
            popover = new NSPopover();
            Application.SetCurrentApplication(new Core.App());
        }

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

        private void StatusItem_Click(object sender, EventArgs e)
        {
            switch (NSApplication.SharedApplication.CurrentEvent.Type)
            {
                // When the tray item is clicked on, we want it to toggle the popover
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

                // Show the menu
                case NSEventType.RightMouseDown:
                    statusItem.PopUpStatusItemMenu(statusMenu);
                    break;
            }
        }
    }

    public class PopoverDelegate : NSPopoverDelegate
    {
        private readonly object appLock;

        public PopoverDelegate()
        {
            appLock = new object();
        }

        public override void DidClose(NSNotification notification)
        {
            lock (appLock)
            {
                Application.Current.SendSleep();
            }
        }

        public override void DidShow(NSNotification notification)
        {
            lock (appLock)
            {
                Application.Current.SendResume();
            }
        }
    }
}
