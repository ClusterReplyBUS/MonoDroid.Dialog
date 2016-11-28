using System;
using System.IO;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class PhotoElement : Element
	{
		private TextView _caption;
		private ImageButton _imageBtn;

		public Bitmap Value { get; set;}
		public string Base64Value
		{
			get
			{
				Bitmap image = Value;
				if (Value != null)
				{
					using (var mem = new MemoryStream())
					{
						image.Compress(Bitmap.CompressFormat.Jpeg, 20, mem);
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

		public PhotoElement(string caption, Bitmap value)
			: base(caption, (int)DroidResources.ElementLayout.dialog_photo)
		{
			Value = value;
		}

		private Context _context;
		public override View GetView(Context context, View convertView, ViewGroup parent)
		{
			_context = context;
			View view = DroidResources.LoadPhotoElementLayout(context, convertView, parent, LayoutId, out _caption, out _imageBtn );

			if (view != null)
			{
				_caption.Text = Caption;
				if (Value != null && _imageBtn != null)
				{
					int currentBitmapWidth = Value.Width;
					int currentBitmapHeight = Value.Height;

					int ivWidth = _imageBtn.Width;
					int ivHeight = _imageBtn.Height;
					int newWidth = ivWidth;

					var newHeight = (int)Math.Floor((double)currentBitmapHeight * ((double)newWidth / (double)currentBitmapWidth));

					Bitmap newbitMap = Bitmap.CreateScaledBitmap(Value, newWidth, newHeight, true);
					_imageBtn.SetImageBitmap(newbitMap);
				}
				if (_imageBtn != null)
				{
					_imageBtn.Click -= _imageBtn_Click;
					_imageBtn.Click += _imageBtn_Click; 
				}
			}
			return view;
		}

		void _imageBtn_Click(object sender, EventArgs e)
		{
			PhotoActivity.Instance.Save += (s, ea) =>
			{
				Value = ea.Value;
				if (Value != null && _imageBtn != null)
				{
					_imageBtn.SetImageBitmap(Value);
				}
			};

			((Android.App.Activity)_context).StartActivity(typeof(PhotoActivity));
		}
	}
}
