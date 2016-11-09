
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SignaturePad;

namespace MonoDroid.Dialog
{
	[Activity(Label = "SignatureActivity")]
	public class SignatureActivity : Activity
	{
		private static volatile SignatureActivity _instance;

		public SignatureActivity():base()
		{
			if (_instance != null)
				this.SignatureSaved = _instance.SignatureSaved;
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
		public string SignatureBase64 { get; set; }
		public event EventHandler SignatureSaved;

		private void OnSignatureSaved()
		{
			if (SignatureSaved != null)
			{
				SignatureSaved(this, null);
			}
		}


		private SignaturePadView _signature;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			 _signature = new SignaturePadView(this)
			{
				//SignatureLineColor=Color.Blue,
				//StrokeColor = Color.Red,

				//StrokeWidth = 10f,
				//BackgroundColor=Color.Yellow,
				//LineWidth = 3f
			};
			_signature.ClearLabel.SetBackgroundColor(Color.Red);
			_signature.ClearLabel.SetTextColor(Color.Blue);

			AddContentView(_signature,
			               new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent));
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



			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{

			switch (item.ItemId)
			{
				case Resource.Id.action_done:
					Bitmap image = _signature.GetImage();
					using (var mem = new MemoryStream())
					{
					 image.Compress(Bitmap.CompressFormat.Png, 20, mem);
						byte[] byteArray = mem.ToArray();
						SignatureBase64 = Convert.ToBase64String(byteArray);
						OnSignatureSaved();
					}
					Finish();
					break;

				case Android.Resource.Id.Home: //Tasto Back con Freccia laterale a sinistra

					Finish();
					break;
			
			}

			return base.OnOptionsItemSelected(item);
		}
	}
}
