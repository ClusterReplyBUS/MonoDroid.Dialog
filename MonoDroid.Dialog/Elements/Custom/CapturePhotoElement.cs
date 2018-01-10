using System;
using System.IO;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class CapturePhotoElement : Element
	{
		private TextView _caption;
		private ImageButton _imageBtn;

		private string _title { get; set; }
		public Bitmap Value { get; set;}
		//public bool IsMandatory { get; set; }

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
					return null;	
				//	return string.Empty;
				}
			}
			set
			{
				if (value != null)
				{
					byte[] encodedDataAsBytes = Convert.FromBase64String(value);
					Value = BitmapFactory.DecodeByteArray(encodedDataAsBytes, 0, encodedDataAsBytes.Length);
				}

				}
		}

		protected bool _showSelector = false;
		protected string _selectorDoneLabel = "Done";
		protected string _selectorTakePhotoLabel = "Take photo";
		protected string _selectorPickImageLabel = "Pick image";
		protected bool _isReadonly = false;
		protected string _deleteButton = "";

		public CapturePhotoElement(string caption, string base64value, bool showSelector, string selectorTakePhotoLabel, string selectorPickImageLabel,string deletebutton,bool isReadonly)
			: base(caption, (int)DroidResources.ElementLayout.dialog_photo)
		{
			this.Base64Value = base64value;
			this._showSelector = showSelector;
			if (!string.IsNullOrWhiteSpace(selectorPickImageLabel))
				this._selectorPickImageLabel = selectorPickImageLabel;
			if (!string.IsNullOrWhiteSpace(selectorTakePhotoLabel))
				this._selectorTakePhotoLabel = selectorTakePhotoLabel;
			if (!string.IsNullOrWhiteSpace(deletebutton))
				this._deleteButton = deletebutton;
			this._isReadonly = isReadonly;
		}
		public CapturePhotoElement(string caption, string base64value) : this(caption, base64value, false, null, null,null,false)
		{
		}

		private Context _context;
		public override View GetView(Context context, View convertView, ViewGroup parent)
		{
			_context = context;
			View view = DroidResources.LoadPhotoElementLayout(context, convertView, parent, LayoutId, out _caption, out _imageBtn );

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

		void _imageBtn_Click(object sender, EventArgs e)
		{
			CapturePhotoActivity.Instance.TitleActivity = _title;
			CapturePhotoActivity.Instance.Save += (s, ea) =>
			{
				Value = ea.Value;
				if (Value != null && _imageBtn != null)
				{
					_imageBtn.SetImageBitmap(Value);
					_caption.SetBackgroundColor(Color.ParseColor("#FAFAD2"));
				}
				else if (Value == null)
				{
					_imageBtn.SetImageBitmap(Value);
				}
			};
			CapturePhotoActivity.Instance.PickImageLabel = _selectorPickImageLabel;
			CapturePhotoActivity.Instance.TakePhotoLabel = _selectorTakePhotoLabel;
			CapturePhotoActivity.Instance.ShowSelector = _showSelector;
			CapturePhotoActivity.Instance.DeleteButtonLabel = _deleteButton;
			((Android.App.Activity)_context).StartActivity(typeof(CapturePhotoActivity));
		}
	}
}
