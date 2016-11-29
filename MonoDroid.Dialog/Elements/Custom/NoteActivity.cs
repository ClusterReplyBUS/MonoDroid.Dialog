
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
	[Activity(Label = "NoteActivity")]

	public class NoteActivity : Activity
	{
		private static volatile NoteActivity _instance;

		public NoteActivity() : base()
		{
			if (_instance != null)
			{
				this.TextNote = _instance.TextNote;
				this.NoteSaved = _instance.NoteSaved;
				this.TitleActivity = _instance.TitleActivity;
				this.SaveButton = _instance.SaveButton;
			}
			_instance = this;
		}


		public static NoteActivity Instance
		{
			get
			{
				if (_instance == null)
					_instance = new NoteActivity();
				return _instance;
			}
		}

		public string TextNote { get; set; }
		public string SaveButton { get; set; }
		public string TitleActivity { get; set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			EditText multiline = new EditText(this);
			multiline.SetMinWidth(100);
			multiline.SetBackgroundColor(Android.Graphics.Color.White);
			if (!string.IsNullOrEmpty(TextNote))
			{
				multiline.Text = TextNote;
			}
			SetContentView(multiline);
			// Create your application here
		}

		public event EventHandler NoteSaved;

		private void OnNoteSaved()
		{
			if (NoteSaved != null)
			{
				NoteSaved(this, null);
			}
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
					OnNoteSaved();
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
	}
}
