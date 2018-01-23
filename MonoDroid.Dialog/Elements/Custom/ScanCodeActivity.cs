using System;
using Android.App;
using Android.OS;
using ZXing.Mobile;

namespace MonoDroid.Dialog
{
    [Activity(Label = "ScanCodeActivity")]
    public class ScanCodeActivity : Activity
    {

        private static volatile ScanCodeActivity _instance;

        public ScanCodeActivity() : base()
        {
            if (_instance != null)
            {
                this.ScannerSaved = _instance.ScannerSaved;
                this.TitleActivity = _instance.TitleActivity;
            }
            _instance = this;
        }
        public static ScanCodeActivity Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ScanCodeActivity();
                return _instance;
            }
        }

        public event EventHandler<ScannerEventArgs> ScannerSaved;
        public string TitleActivity { get; set; }

        private void OnScannerSaved(string res)
        {
            if (ScannerSaved != null)
            {
                ScannerSaved(this, new ScannerEventArgs(res));
            }
        }


        private MobileBarcodeScanner _scanner;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _scanner = new MobileBarcodeScanner();

            var result = await _scanner.Scan();

            if (result != null)
            {
                OnScannerSaved(result.Text);
                Finish();
            }
        }

        protected override void OnResume()
        {
            Finish();
            base.OnResume();
        }
        public override bool OnKeyDown(Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
        {
            if (e.KeyCode == Android.Views.Keycode.Back || e.KeyCode == Android.Views.Keycode.Home)
            {
                Finish();
            }
            return base.OnKeyDown(keyCode, e);
        }





        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            Window.SetTitle("Scanner");

        }



        public class ScannerEventArgs : EventArgs
        {
            public string ScannerResult { get; private set; }
            public ScannerEventArgs(string arg)
            {
                ScannerResult = arg;
            }
        }
    }
}
