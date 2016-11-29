using System;
using Android.App;

namespace MonoDroid.Dialog
{
	public class SelectableMultilineEntryElement :ButtonElement //ReadonlyElement
	{
		public bool IsMandatory { get; set; }
		public SelectableMultilineEntryElement(string caption, string value, string saveLabel)
			: base(caption, null)
		{
			
		}

		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			this.Click+=()=>
			{

				((Activity)context).StartActivity(typeof(NoteActivity));
			
			
			};

			var view=base.GetView(context, convertView, parent);
			return view;
		}

		public override void Selected()
		{
			base.Selected();
		}
	}
}
