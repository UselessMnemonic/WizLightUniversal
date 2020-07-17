// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace WizLightUniversal.macOS
{
	[Register ("Container")]
	partial class Container
	{
		[Outlet]
		AppKit.NSButton BackButton { get; set; }

		[Outlet]
		AppKit.NSView Content { get; set; }

		[Outlet]
		AppKit.NSButton PreferencesButton { get; set; }

		[Outlet]
		AppKit.NSButton RefreshButton { get; set; }

		[Outlet]
		AppKit.NSTextField TitleLabel { get; set; }

		[Action ("Back_Clicked:")]
		partial void Back_Clicked (AppKit.NSButton sender);

		[Action ("Preferences_Clicked:")]
		partial void Preferences_Clicked (AppKit.NSButton sender);

		[Action ("Refresh_Clicked:")]
		partial void Refresh_Clicked (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (BackButton != null) {
				BackButton.Dispose ();
				BackButton = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (PreferencesButton != null) {
				PreferencesButton.Dispose ();
				PreferencesButton = null;
			}

			if (RefreshButton != null) {
				RefreshButton.Dispose ();
				RefreshButton = null;
			}

			if (Content != null) {
				Content.Dispose ();
				Content = null;
			}
		}
	}
}
