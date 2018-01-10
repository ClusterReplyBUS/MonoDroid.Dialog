using System;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using SignaturePad;

namespace MonoDroid.Dialog
{
	[Activity(Label = "SignatureActivity")]
	public class SignatureActivity : Activity
	{
		private const float SIGNATURE_PROPORTION = 0.5f;
		private static volatile SignatureActivity _instance;

		public SignatureActivity() : base()
		{
			if (_instance != null)
			{
				this.SignatureSaved = _instance.SignatureSaved;
				this.Disclaimer = _instance.Disclaimer;
				this.SaveButton = _instance.SaveButton;
				this.TitleActivity = _instance.TitleActivity;
				this.SignatureImage = _instance.SignatureImage;
			}
			_instance = this;
		}
		public static SignatureActivity Instance
		{
			get
			{
				if (_instance == null)
					_instance = new SignatureActivity();
				return _instance;
			}
		}
		public Bitmap SignatureImage { get; set; }

		public event EventHandler<BitmapEventArgs> SignatureSaved;
		public string TitleActivity { get; set; }

		private void OnSignatureSaved(Bitmap signature)
		{
			if (SignatureSaved != null)
			{
				SignatureSaved(this, new BitmapEventArgs(signature));
			}
		}

		public string Disclaimer;
		public string SaveButton;
		private SignaturePadView _signature;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			ActionBar actionBar = ActionBar;
			actionBar.SetBackgroundDrawable(new ColorDrawable(Color.ParseColor(Colors.PrimaryColor))); // set your desired color
			int heightStatusBar = getStatusBarHeight();
			Display display = WindowManager.DefaultDisplay;
			Point size = new Point();
			display.GetSize(size);
			int width = size.X;
			int height = (size.Y)-(int)(heightStatusBar*1.1);



			_signature = new SignaturePadView(this)
			{
				SignatureLineColor = Color.Black,
				StrokeColor = Color.Black,
				StrokeWidth = 5f,
				BackgroundColor = Color.White,
				//LineWidth = 3f
			};
			_signature.ClearLabelText = "X";
			_signature.ClearLabel.SetBackgroundColor(Color.Transparent);
			_signature.ClearLabel.SetTextColor(Color.Red);
			_signature.ClearLabel.SetWidth(100);
			_signature.ClearLabel.SetHeight(100);

			var _disclaimer = new TextView(this);
			_disclaimer.Text =Disclaimer;
			_disclaimer.SetPadding(10, 0, 0, 0);
			_disclaimer.SetTextColor(Color.Black);
			_disclaimer.SetBackgroundColor(Color.LightGray);
			_disclaimer.AutoLinkMask = Android.Text.Util.MatchOptions.All;
			_disclaimer.Clickable = true;
			_disclaimer.SetLinkTextColor(Color.Blue);
			_disclaimer.LinksClickable=true;
			_disclaimer.MovementMethod = Android.Text.Method.LinkMovementMethod.Instance;
			//if (SignatureBase64 != null)
			//{

			//}

		
			AddContentView(_disclaimer, new ViewGroup.LayoutParams(width,(int)(height*(1f-SIGNATURE_PROPORTION))));
			_signature.SetY((int)(height * (1f - SIGNATURE_PROPORTION)) + 1);
	
			AddContentView(_signature,
			               new ViewGroup.LayoutParams((width), (int)(height *(SIGNATURE_PROPORTION))-1));
			//   new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent));
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			MenuInflater inflater = MenuInflater;
			inflater.Inflate(Resource.Layout.Menu, menu);
			return true;
		}

		public override bool OnPrepareOptionsMenu(IMenu menu)
		{
			var btnDone = menu.FindItem(Resource.Id.action_done);
			btnDone.SetTitle(SaveButton);
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.action_done:
					if (!_signature.IsBlank)
					{
						var img=_signature.GetImage();
						
						Bitmap.Config conf = Bitmap.Config.Argb8888; // see other conf types
						Bitmap bmp = Bitmap.CreateBitmap(img.Width, img.Height, conf); // this creates a MUTABLE bitmap
						bmp.EraseColor(Color.White);
						Canvas canvas = new Canvas(bmp);
						canvas.DrawBitmap(img, 0, 0, null);
						//SignatureImage.EraseColor(Color.Green);
						SignatureImage = bmp;
						OnSignatureSaved(SignatureImage);
					}
					Finish();
					break;

				case Android.Resource.Id.Home: //Tasto Back con Freccia laterale a sinistra

					Finish();
					break;
			}
			return base.OnOptionsItemSelected(item);
		}

		public override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();
			Window.SetTitle(TitleActivity);

		}

		public int getStatusBarHeight()
		{
			int result = 0;
			int resourceId = Resources.GetIdentifier("navigation_bar_height", "dimen", "android");
			int resourceIdStasusBar = Resources.GetIdentifier("status_bar_height", "dimen", "android");
			if (resourceId > 0 || resourceIdStasusBar>0 )
			{
				result = Resources.GetDimensionPixelSize(resourceId) + Resources.GetDimensionPixelSize(resourceIdStasusBar) ;
			}
			return result;
		}


	}
}
