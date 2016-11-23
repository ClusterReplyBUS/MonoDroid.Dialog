using System;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class ReadonlyElement : RightAlignEntryElement
	{

		//public int FontSize { get; set; }
		//public string Value
		//{
		//	get { return _value; }
		//	set { _value = value; if (_text != null) _text.Text = _value; }
		//}
		//private string _value;

		public ReadonlyElement(string caption, string value)
			: base(caption, "", value)
		{

			//Value = value;

		}

		public override View GetView(Context context, View convertView, ViewGroup parent)
		{
			var view =  base.GetView(context, convertView, parent);
			_entry.Enabled = false;
			return view;
		}


		//public override View GetView(Context context, View convertView, ViewGroup parent)
		//{

		//	View view = DroidResources.LoadReadOnlyStringElementLayout(context, convertView, parent, LayoutId, out _caption, out _text);
		//	if (view != null)
		//	{
		//		base.
		//		_caption.Text = Caption;
		//		_caption.TextSize = FontSize;
		//		_text.Text = Value;
		//		//_text.TextSize = FontSize;
		//		if (Click != null)
		//			view.Click += delegate { this.Click(); };
		//	}
		//	return view;
		//}
	}
}
