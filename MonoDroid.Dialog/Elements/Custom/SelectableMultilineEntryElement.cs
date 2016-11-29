using System;
using Android.App;

namespace MonoDroid.Dialog
{
	public class SelectableMultilineEntryElement : ButtonElement //ReadonlyElement
	{
		public bool IsMandatory { get; set; }
		private string _textNote { get; set; }
		public SelectableMultilineEntryElement(string caption, string value, string saveLabel)
			: base(caption, null)
		{

		}

		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			if (this.Click == null)
			{
				this.Click += () =>
				  {
					  NoteActivity.Instance.TextNote = _textNote;
					  NoteActivity.Instance.NoteSaved += (sender, e) =>
					   {
						   _textNote = ((NoteActivity)sender).TextNote;
					   };
					  ((Activity)context).StartActivity(typeof(NoteActivity));
				  };
			}
			var view = base.GetView(context, convertView, parent);
			return view;
		}

		public override void Selected()
		{
			base.Selected();
		}

		public event EventHandler NoteSaved;

		private void OnNoteSaved()
		{
			if (NoteSaved != null)
			{
				NoteSaved(this, null);
			}
		}
	}
}
