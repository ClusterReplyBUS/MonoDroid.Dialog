using System;
namespace MonoDroid.Dialog
{
		public class ReadonlyElement : StringElement
		{
			public ReadonlyElement(string caption, string value)
				: base(caption, value)
			{
			}
		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			return base.GetView(context, convertView, parent);
		}
	}
}
