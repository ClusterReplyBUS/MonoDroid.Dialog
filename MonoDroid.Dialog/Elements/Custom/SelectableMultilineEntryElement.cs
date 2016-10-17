using System;
namespace MonoDroid.Dialog
{
	public class SelectableMultilineEntryElement : ReadonlyElement
	{
		public SelectableMultilineEntryElement(string caption, string value, string saveLabel)
		   : base(caption, value)
		{
			
		}

		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			return base.GetView(context, convertView, parent);
		}

		public override void Selected()
		{
			base.Selected();
		}
	}
}
