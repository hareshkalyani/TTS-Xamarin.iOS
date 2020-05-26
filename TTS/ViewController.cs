namespace mintyfusion.studio.xamarin.ios.TTS
{
    #region Namespace
    using AVFoundation;
    using Foundation;
    using System;
    using UIKit;
    #endregion

    #region Class
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

            // TextField events to resign keyboard
            textField.ShouldReturn += (sender) =>
            {
                textField.ResignFirstResponder();

                return true;
            };

            // speakButton click event to perform text to speech
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
            AVSpeechSynthesisVoice[] voices = TextToSpeech.SharedInstance.GetVoices();

            return voices.Length;
        }

        [Export("pickerView:titleForRow:forComponent:")]
        public string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            AVSpeechSynthesisVoice[] voices = TextToSpeech.SharedInstance.GetVoices();

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
    #endregion
}