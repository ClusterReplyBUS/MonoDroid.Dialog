using System;
using System.IO;
using Android.App;
using Android.Graphics;

namespace MonoDroid.Dialog
{
	public class SignatureElement : ButtonElement
	{
		public string SignatureBase64 { 
			get
			{ 
				Bitmap image = Value;
				using (var mem = new MemoryStream())
				{
					image.Compress(Bitmap.CompressFormat.Jpeg, 20, mem);
					byte[] byteArray = mem.ToArray();
				 	return Convert.ToBase64String(byteArray);
				}
			}
			set
			{ 
				byte[] encodedDataAsBytes = Convert.FromBase64String(value);
				Value=BitmapFactory.DecodeByteArray(encodedDataAsBytes, 0, encodedDataAsBytes.Length);
			}
		}

		public new Bitmap Value { get; set; }


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
					 Value = SignatureActivity.Instance.SignatureImage;

				 };
				 ((Activity)context).StartActivityForResult(typeof(SignatureActivity), 0);
				 
			 };
			return base.GetView(context, convertView, parent);
		}
	}
}
