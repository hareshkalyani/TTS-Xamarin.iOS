namespace mintyfusion.studio.xamarin.ios.TTS
{
    #region Namespace
    using AVFoundation;
    using Foundation;
    using System;
    using UIKit;
    #endregion

    #region Class
    public class TextToSpeech
    {
        #region Fields
        private static TextToSpeech instance = null;

        private static object lockObject = new object();

        private AVSpeechSynthesizer speechSynthesizer = null;

        private FinishEvent audioFinishHandler = null;
        #endregion

        #region Constructor
        public TextToSpeech()
        {
            Configure();
        }
        #endregion

        #region Properties
        public bool IsPlaying { get; set; }
        #endregion

        #region Public Static Methods
        public static void Initialize()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                        instance = new TextToSpeech();
                }
            }
        }

        public static void ReInitialize()
        {
            instance = null;
        }

        public static TextToSpeech SharedInstance
        {
            get
            {
                Initialize();

                return instance;
            }
        }
        #endregion

        #region Public Methods
        public void Speak(string text, FinishEvent audioFinishHandler = null)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("text");

            Speak(text, string.Empty, audioFinishHandler);
        }

        public void Speak(string text, string voiceTypeIdentifier, FinishEvent audioFinishHandler = null)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("text");

            if (IsPlaying)
                return;

            this.audioFinishHandler = audioFinishHandler;

            // Play with Default Voice
            IsPlaying = true;

            AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.PlayAndRecord,
            AVAudioSessionCategoryOptions.AllowBluetooth | AVAudioSessionCategoryOptions.AllowBluetoothA2DP);

            AVAudioSession.SharedInstance().OverrideOutputAudioPort(AVAudioSessionPortOverride.Speaker, out NSError error);

            AVSpeechUtterance speechUtterance = new AVSpeechUtterance(text);

            if (!string.IsNullOrEmpty(voiceTypeIdentifier))
                speechUtterance.Voice = AVSpeechSynthesisVoice.FromIdentifier(voiceTypeIdentifier);

            speechSynthesizer.SpeakUtterance(speechUtterance);
        }

        public void StopPlaying(FinishEvent audioFinishHandler = null)
        {
            if (speechSynthesizer != null && IsPlaying)
            {
                IsPlaying = false;
                speechSynthesizer.StopSpeaking(AVSpeechBoundary.Immediate);
            }
            else
                audioFinishHandler?.Invoke();
        }

        public AVSpeechSynthesisVoice[] GetVoices()
        {
            return AVSpeechSynthesisVoice.GetSpeechVoices();
        }
        #endregion

        #region Private Methods
        private void Configure()
        {
            speechSynthesizer = new AVSpeechSynthesizer();

            speechSynthesizer.DidFinishSpeechUtterance += SpeechSynthesizer_DidFinishSpeechUtterance;

            speechSynthesizer.DidCancelSpeechUtterance += SpeechSynthesizer_DidFinishSpeechUtterance;

            NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.WillResignActiveNotification, (obj) =>
            {
                StopPlaying();
            });
        }

        private void SpeechSynthesizer_DidFinishSpeechUtterance(object sender, AVSpeechSynthesizerUteranceEventArgs e)
        {
            IsPlaying = false;

            audioFinishHandler?.Invoke();
        }
        #endregion

        #region Delegate
        public delegate void FinishEvent();
        #endregion
    }
    #endregion
}