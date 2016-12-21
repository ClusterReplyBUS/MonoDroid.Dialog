using System;
using Android.Graphics;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class RightAlignEntryElement : EntryElement
	{
		public bool IsMandatory { get; set; }
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

			if (!string.IsNullOrWhiteSpace(Value))
			{
				_entry.SetBackgroundColor(Color.ParseColor("#FAFAD2"));
			}
			return view;
		}
	}
}
