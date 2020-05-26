using Foundation;
using System;
using UIKit;

namespace TTS
{
    public partial class ViewController : UIViewController, IUIPickerViewDataSource, IUIPickerViewDelegate
    {
        #region Constructor
        public ViewController(IntPtr handle)
            : base(handle) { }
        #endregion

        #region Override Methods
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Setup picker for different voices selection
            pickerView.DataSource = this;
            pickerView.WeakDelegate = this;

            textField.ShouldReturn += (sender) =>
            {
                textField.ResignFirstResponder();

                return true;
            };

            speakButton.TouchUpInside += (sender, e) =>
            {
                textField.ResignFirstResponder();

                if (!string.IsNullOrWhiteSpace(textField.Text))
                {
                    TextToSpeech.SharedInstance.Speak(textField.Text, () =>
                    {
                        Console.WriteLine("Audio finished");
                    });
                }
            };
        }
        #endregion

        #region PickerView Datasource and Delegate
        public nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            var voices = TextToSpeech.SharedInstance.GetVoices();

            return voices.Length;
        }

        [Export("pickerView:titleForRow:forComponent:")]
        public string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            var voices = TextToSpeech.SharedInstance.GetVoices();

            return voices[row].Name;
        }

        [Export("pickerView:didSelectRow:inComponent:")]
        public void Selected(UIPickerView pickerView, nint row, nint component)
        {
            var voices = TextToSpeech.SharedInstance.GetVoices();

            if (!string.IsNullOrWhiteSpace(textField.Text))
            {
                TextToSpeech.SharedInstance.Speak(textField.Text, voices[row].Identifier, () =>
                {
                    Console.WriteLine("Audio finished");
                });
            }
        }
        #endregion
    }
}