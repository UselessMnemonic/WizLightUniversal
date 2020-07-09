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
		AppKit.NSView Content { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Content != null)
            {
				Content.Dispose ();
				Content = null;
            }
		}
	}
}
