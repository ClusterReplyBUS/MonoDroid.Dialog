using System;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class RightAlignEntryElement : EntryElement
	{
		//public bool IsMandatory { get; set; }
		public RightAlignEntryElement(string caption, string placeholder, string value)
			: base(caption, placeholder, value)
		{

		}

		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			var view = base.GetView(context, convertView, parent);

			if (this.IsMandatory && _label != null && !_label.Text.EndsWith("*", StringComparison.InvariantCulture))
			{
				_label.Text += "*";
			}

            if (this.IsMissing)
                _label.SetTextColor(Color.ParseColor(Colors.MissingRed));

            if (!string.IsNullOrWhiteSpace(Value))
			{
				_entry.SetBackgroundColor(Color.ParseColor("#FAFAD2"));
			}



				

			return view;
		}

		//protected override void changeStatusItem(View item)
		//{
		//	//var p = ((ViewGroup)view).FocusedChild;
		//	if (item != null)
		//	{
		//		item.FocusChange += View_FocusChange;
		//	}

		//	//item.SetBackgroundColor(Color.Green);
		//}

		//void View_FocusChange(object sender, View.FocusChangeEventArgs e)
		//{
		//	var view = sender as Android.Views.View;
		//	if (e.HasFocus)
		//	{
		//		var inputMethodManager = view.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
		//		inputMethodManager.ShowSoftInput(view,ShowFlags.Forced);
		//	}
		//	if(!e.HasFocus)
		//	{
		//		var inputMethodManager = view.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
		//		inputMethodManager.HideSoftInputFromWindow(view.WindowToken, 0);
		//	}
		//}
	}
}
