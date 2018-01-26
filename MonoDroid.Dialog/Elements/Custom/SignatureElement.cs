using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class SignatureElement : Element
	{
		private TextView _caption;
		private ImageButton _imageBtn;
		private string _disclaimer;
		private string _savebutton;
		protected bool _isReadonly = false;

		//public bool IsMandatory { get; set; }
		public string SignatureBase64
		{
			get
			{
				Bitmap image = Value;
				if (Value != null)
				{
					using (var mem = new MemoryStream())
					{
						image.Compress(Bitmap.CompressFormat.Jpeg, 100, mem);

						byte[] byteArray = mem.ToArray();
						return Convert.ToBase64String(byteArray);
					}
				}
				else
				{
					return string.Empty;
				}
			}
			set
			{
				byte[] encodedDataAsBytes = Convert.FromBase64String(value);
				Value = BitmapFactory.DecodeByteArray(encodedDataAsBytes, 0, encodedDataAsBytes.Length);
			}
		}

		public new Bitmap Value { get; set; }

		public SignatureElement(string caption, string disclaimer, string saveButtonLabel)
			: base(caption, (int)DroidResources.ElementLayout.dialog_photo)
		{
			this._disclaimer = disclaimer;
			this._savebutton = saveButtonLabel;
			Caption = caption;
		}

		private Context _context;
		public override View GetView(Context context, View convertView, ViewGroup parent)
		{
			_context = context;
			View view = DroidResources.LoadPhotoElementLayout(context, convertView, parent, LayoutId, out _caption, out _imageBtn);

			if (view != null)
			{
				if (this.IsMandatory && Caption != null && !Caption.EndsWith("*", StringComparison.InvariantCulture))
					this.Caption += "*";

				_caption.Text = Caption;
				if (Value != null && _imageBtn != null)
				{
					int currentBitmapWidth = Value.Width;
					int currentBitmapHeight = Value.Height;

					int ivWidth = _imageBtn.Width;
					int ivHeight = _imageBtn.Height;
					if (ivWidth == 0 && ivHeight == 0)
					{
						ivWidth = 100;
						ivHeight = 100;
					}
					int newWidth = ivWidth;
					var newHeight = (int)Math.Floor((double)currentBitmapHeight * ((double)newWidth / (double)currentBitmapWidth));

					Bitmap newbitMap = Bitmap.CreateScaledBitmap(Value, newWidth, newHeight, true);
					_imageBtn.SetImageBitmap(newbitMap);
					_caption.SetBackgroundColor(Color.ParseColor("#FAFAD2"));
				}

				if (_imageBtn != null)
				{
					if (_isReadonly)
					{
						_imageBtn.Enabled = false;
					}
					else
					{
						_imageBtn.Click -= _imageBtn_Click;
						_imageBtn.Click += _imageBtn_Click;
					}
				}
			}
			return view;
		}

		void _imageBtn_Click(object s, EventArgs ea)
		{
			SignatureActivity.Instance.Disclaimer = _disclaimer;
			SignatureActivity.Instance.SaveButton = _savebutton;
			SignatureActivity.Instance.TitleActivity = Caption;
			SignatureActivity.Instance.SignatureSaved += (sender, e) =>
			 {
				 //Value = SignatureActivity.Instance.SignatureImage;
				 Value = e.Value;
				 if (Value != null && _imageBtn != null)
				 {
					// _imageBtn.SetBackgroundColor(Color.White);
					 _imageBtn.SetImageBitmap(Value);
					
					 _caption.SetBackgroundColor(Color.ParseColor("#FAFAD2"));
				 }
			 };
			((Activity)_context).StartActivityForResult(typeof(SignatureActivity), 0);
		}
	}
}
