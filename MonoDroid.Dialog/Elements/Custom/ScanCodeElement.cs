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
        public string cancelLabel { get; set; }
        public string flashLabel { get; set; }
        public string flashOnLabel { get; set; }
        public string flashOffLabel { get; set; }

        public ScanCodeElement(string caption,string cancelLabel,string flashLabel,string flashOnLabel,string flashOffLabel) : base(caption, null)
        {
            this.cancelLabel = cancelLabel;
            this.flashLabel = flashLabel;
            this.flashOnLabel = flashOnLabel;
            this.flashOffLabel = flashOffLabel;
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
                ScanCodeActivity.Instance.FlashOffLabel = flashOffLabel;
                ScanCodeActivity.Instance.FlashOnLabel = flashOnLabel;
                ScanCodeActivity.Instance.CancelLabel = cancelLabel;
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
