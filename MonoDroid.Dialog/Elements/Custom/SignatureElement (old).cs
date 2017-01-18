//using System;
//using System.IO;
//using Android.App;
//using Android.Graphics;

//namespace MonoDroid.Dialog
//{
//	public class SignatureElementOld : ButtonElement
//	{
//		public bool Mandatory { get; set; }
//		public string SignatureBase64
//		{
//			get
//			{
//				Bitmap image = Value;
//				if (Value != null)
//				{
//					using (var mem = new MemoryStream())
//					{
//						image.Compress(Bitmap.CompressFormat.Jpeg, 20, mem);
//						byte[] byteArray = mem.ToArray();
//						return Convert.ToBase64String(byteArray);
//					}
//				}
//				else
//				{
//					return string.Empty;
//				}
//			}
//			set
//			{
//				byte[] encodedDataAsBytes = Convert.FromBase64String(value);
//				Value = BitmapFactory.DecodeByteArray(encodedDataAsBytes, 0, encodedDataAsBytes.Length);
//			}
//		}

//		public new Bitmap Value { get; set; }
//		private string _disclaimer;
//		private string _savebutton;

//		public SignatureElement(string caption, string disclaimer, string saveButtonLabel)
//			: base(caption, null)
//		{
//			this._disclaimer = disclaimer;
//			this._savebutton = saveButtonLabel;
//			Caption = caption;
//		}

//		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
//		{
//			if (this.Mandatory && Caption != null && !Caption.EndsWith("*", StringComparison.InvariantCulture))
//				this.Caption += "*";

//			if (this.Click == null)
//			{
//				this.Click += () =>
//				 {
//					 SignatureActivity.Instance.Disclaimer = _disclaimer;
//					 SignatureActivity.Instance.SaveButton = _savebutton;
//					 SignatureActivity.Instance.TitleActivity = Caption;
//					 SignatureActivity.Instance.SignatureSaved += (sender, e) =>
//					  {
//						  Console.WriteLine("Signature has been saved");
//						  //Value = SignatureActivity.Instance.SignatureImage;
//						  Value = e.Value;
//					  };
//					 ((Activity)context).StartActivityForResult(typeof(SignatureActivity), 0);
//				 };
//			}
//			var view = base.GetView(context, convertView, parent);
//			return view;
//		}
//	}
//}
