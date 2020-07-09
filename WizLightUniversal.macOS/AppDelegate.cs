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
        private NSPopover popover;

        public AppDelegate()
        {
            // Init Xamarin.Forms
            Forms.Init();

            // Create the tray item w/ icon and handler
            statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(NSStatusItemLength.Square);
            statusItem.Button.Image = NSImage.ImageNamed("TrayIcon_18_18.ico");

            // The core app will run under this app delegate
            popover = new NSPopover();
            Application.SetCurrentApplication(new Core.App());
        }

        private void StatusItemClick(object sender, EventArgs e)
        {
            // When the tray item is clicked on, we want it to toggle the popover
            if (!popover.Shown)
            {
                popover.Show(statusItem.Button.Bounds, statusItem.Button, NSRectEdge.MaxYEdge);
            }
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Create container for popover
            Container container = Container.FreshController();
            container.SetContent(Application.Current.MainPage.CreateViewController());

            // Furnish popover with content
            popover.ContentViewController = container;
            popover.Behavior = NSPopoverBehavior.Transient;
            popover.Delegate = new PopoverDelegate();
            popover.Animates = false;
            popover.SetAppearance(NSAppearance.GetAppearance(NSAppearance.NameLightContent));

            // Enable the tray item only after the app has launched
            statusItem.Button.Activated += StatusItemClick;
            Application.Current.SendStart();
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
            statusItem.Dispose();
            popover.Dispose();
        }
    }

    public class PopoverDelegate : NSPopoverDelegate
    {
        public override void DidClose(NSNotification notification)
        {
            Application.Current.SendSleep();
        }

        public override void DidShow(NSNotification notification)
        {
            Application.Current.SendResume();
        }
    }
}
