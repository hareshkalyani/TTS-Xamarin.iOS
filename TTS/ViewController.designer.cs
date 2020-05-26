// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace mintyfusion.studio.xamarin.ios.TTS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIPickerView pickerView { get; set; }

		[Outlet]
		UIKit.UIButton speakButton { get; set; }

		[Outlet]
		UIKit.UITextField textField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (pickerView != null) {
				pickerView.Dispose ();
				pickerView = null;
			}

			if (speakButton != null) {
				speakButton.Dispose ();
				speakButton = null;
			}

			if (textField != null) {
				textField.Dispose ();
				textField = null;
			}
		}
	}
}
