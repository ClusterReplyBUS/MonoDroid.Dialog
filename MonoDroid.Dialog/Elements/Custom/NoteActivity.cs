
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
		public override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();
			//Window.SetTitle(TitleActivity);

		}


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			EditText multiline = new EditText(this);
			multiline.SetMinWidth(100);
			multiline.SetBackgroundColor(Android.Graphics.Color.White);
			SetContentView(multiline);
			// Create your application here
		}
	}
}
