using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using static MonoDroid.Dialog.ScanCodeActivity;

namespace MonoDroid.Dialog
{
    public class ScanCodeElement : ButtonElement
    {


        public string ScannerResult { get; set; }
        public ScanCodeElement(string caption) : base(caption, null)
        {

        }


        public override void Selected()
        {


        }


        public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {

            this.Click += () =>
            {
                ScanCodeActivity.Instance.ScannerSaved -= OnScannerSaved;
                ScanCodeActivity.Instance.ScannerSaved += OnScannerSaved;
                ((Activity)context).StartActivity(typeof(ScanCodeActivity));
            };


            return base.GetView(context, convertView, parent);
        }
        void OnScannerSaved(object sender, ScannerEventArgs e)
        {
            ScannerResult = e.ScannerResult;
            if (!string.IsNullOrEmpty(ScannerResult))
            {
                OnSendResponse(ScannerResult);
            }
        }

        public event EventHandler<ScannerEventArgs> SendResponse;
        private void OnSendResponse(string scanResult)
        {
            if (SendResponse != null)
            {
                SendResponse(this, new ScannerEventArgs(scanResult));
            }
        }


    }

}
