using System;
using Android.App;

namespace MonoDroid.Dialog
{
	public class SignatureElement : ButtonElement
	{
		public string SignatureBase64 { get; set; }
		public SignatureElement(string caption, string disclaimer, string saveButtonLabel)
			: base(caption, null)
		{
		}

		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			this.Click += () =>
			 {
				SignatureActivity.Instance.SignatureSaved += (sender, e) =>
				 {
					 Console.WriteLine("SignatureSaved");
					 SignatureBase64 = SignatureActivity.Instance.SignatureBase64;
				 };
				 ((Activity)context).StartActivityForResult(typeof(SignatureActivity), 0);
				 
			 };
			var view = base.GetView(context, convertView, parent);

			return view;
		}
	}
}
