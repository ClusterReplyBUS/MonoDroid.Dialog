using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using ZXing.Mobile;
using static Android.Support.V4.Widget.DrawerLayout;

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
            _scanner.UseCustomOverlay = true;


            _scanner.AutoFocus();

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            Console.WriteLine("INFLATER : " + inflater);
            //var layout = inflater.Inflate(MonoDroid.Dialog.Resource.Layout.custom_scanner, null);
            
           var layout = inflater.Inflate(2130903071, null);
            Console.WriteLine("LAYOUT : " + layout);
            var flash = ((Button)layout.FindViewById(Resource.Id.buttonZxingFlash));
            if (flash == null)
            {
                flash = (Button)layout.FindViewById(BaseContext.Resources.GetIdentifier("buttonZxingFlash", "id", BaseContext.PackageName));
            }
            if (flash == null)
            {
                flash = ((Button)layout.FindViewById(2131361962));
            }
            if (flash != null)
                flash.Click += (sender, e) =>
                {
                    _scanner.ToggleTorch();
                    if (_scanner.IsTorchOn)
                    {
                        flash.SetText("Flash Off", TextView.BufferType.Normal);
                    }
                    else
                    {
                        flash.SetText("Flash On", TextView.BufferType.Normal);
                    }
                };

            var cancel = (Button)layout.FindViewById(MonoDroid.Dialog.Resource.Id.buttonZxingCancel);
            if (cancel == null)
            {
                cancel = (Button)layout.FindViewById(BaseContext.Resources.GetIdentifier("buttonZxingCancel", "id", BaseContext.PackageName));
            }
            if (cancel == null)
            {
                cancel = ((Button)layout.FindViewById(2131361961));
            }
            if (cancel != null)
            cancel.Click += (sender, e) =>
            {
                _scanner.Cancel();
            };

            _scanner.CustomOverlay = layout;
            MobileBarcodeScanningOptions option = new MobileBarcodeScanningOptions();
            option.TryHarder = true;
            option.TryInverted = true;
            //option.PossibleFormats = new List<ZXing.BarcodeFormat>()
            //{
            //   // ZXing.BarcodeFormat.
            //};
            var result = await _scanner.Scan(option);
            if (result != null)
            {

                OnScannerSaved(result.Text);
                _instance = null;
                Finish();
            }
        }

        protected override void OnResume()
        {
            try
            {
                Finish();
            }
            catch
            {
                Finish();
            }
            finally
            {
                base.OnResume();

            }
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



        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
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
