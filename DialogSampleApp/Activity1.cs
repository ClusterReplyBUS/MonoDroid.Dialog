using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using MonoDroid.Dialog;
using System;

namespace DialogSampleApp
{
	//
	// NOTE: with the new update you will have to add all the dialog_* prefixes to your main application.
	//       This is because the current version of Mono for Android will not add resources from assemblies
	//       to the main application like it does for libraries in Android/Java/Eclipse...  This could
	//       change in a future version (it's slated for 1.0 post release) but for now, just add them 
	//       as in this sample...
	//
	[Activity(Label = "MD.D Sample", MainLauncher = true, WindowSoftInputMode = SoftInput.AdjustPan)]
	public class Activity1 : Activity
	{
		protected void StartNew()
		{
			StartActivity(typeof(Activity2));
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);


			ImageView img = new ImageView(this);
			img.SetImageResource(Resource.Drawable.spadeicon);

			var root = new RootElement("Test Root Elem")
				{
					new Section("Test Header", "Test Footer")
						{
					//		new ButtonElement("DialogActivity", () => StartNew()),
					//new PhotoElement("Photo", null),
							//new StringElement("Do Something", "Foo", () => {
							//	Console.WriteLine("Did Something");
							//}),
							new BooleanElement("Push my button1", true),
					new GridElement("DialogActivity","save"){Mandatory = true},
							new ButtonElement("DialogActivity", () => StartNew()),
							new BooleanElement("Push my button2", true),
							new TextOnlyElement("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis justo orci, porttitor eget malesuada a, elementum vitae mauris. Pellentesque et rhoncus nunc, a condimentum sapien. Sed sed elit justo. Praesent ligula enim, tincidunt non pulvinar vel, vulputate ut risus. Suspendisse varius nunc id vehicula pellentesque. Praesent finibus a leo et imperdiet. Maecenas id augue ac dolor consequat bibendum. Vestibulum cursus tellus eu libero molestie accumsan. Cras vestibulum facilisis commodo. Nunc consequat neque ac augue imperdiet, in facilisis urna ornare. Morbi metus quam, tincidunt ac porttitor sit amet, iaculis nec risus. Vivamus consequat tincidunt condimentum. Aliquam sed nunc eu urna consequat feugiat. Nulla magna felis, molestie nec augue gravida, ornare placerat tortor."),
							new BooleanElement("Push my button15", true),
							new BooleanElement("Push this too", false),
							//new StringElement("Text label", "The Value"),
							//new BooleanElement("Push my button", true),
							//new BooleanElement("Push this too", false),
						},
					//new Section("Part II")
					//	{
					//		new StringElement("This is the String Element", "The Value"),
					//		//new CheckboxElement("Check this out", true),
					//		new EntryElement("Username",""){
					//			Hint = "Enter Login"
					//		},
					//		new EntryElement("Password", "") {
					//			Hint = "Enter Password",
					//			Password = true,
					//		},
					//	}

				};

			var da = new DialogAdapter(this, root);

			var lv = new ListView(this) { Adapter = da };


			SetContentView(lv);
		}
	}
}