using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class SelectableMultilineEntryElement : Element
	{
		//public bool IsMandatory { get; set; }
		private TextView _caption;
		private TextView _value;
		public string Value { get; set; }
		private Context _context { get; set; }
		private string _saveButton { get; set; }

		public SelectableMultilineEntryElement(string caption,bool isMandatory, string value, string saveLabel)
			: base(caption, (int)DroidResources.ElementLayout.dialog_note)
		{
			_saveButton = saveLabel;
			Value = value;
            IsMandatory = isMandatory;
		}

		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			_context = context;
			var view = DroidResources.LoadNoteElementLayout(context, convertView, parent, LayoutId, out _caption, out _value);
			if (view != null)
			{
                if (this.IsMissing)
                    _caption.SetTextColor(Color.ParseColor(Colors.MissingRed));

                _caption.Text = Caption;
                if (IsMandatory && !_caption.Text.EndsWith("*"))
                {
                    _caption.Text = _caption.Text + "*";
                }

                if (this.IsMissing)
                    _caption.SetTextColor(Color.ParseColor("#db0000"));

                if (string.IsNullOrEmpty(Value))
				{
					_value.Text = string.Empty;
				}
				else {
					_value.Text = Value;
					_value.SetBackgroundColor(Color.ParseColor("#FAFAD2"));
				}

				if (_caption != null)
				{
					view.Click -= HandleEventHandler;
					view.Click += HandleEventHandler;
				}
			}
			return view;
		}

		void HandleEventHandler(object sender, EventArgs e)
		{
			NoteActivity.Instance.TextNote = Value;
			NoteActivity.Instance.SaveButton = _saveButton;
			NoteActivity.Instance.TitleActivity = _caption.Text;
			NoteActivity.Instance.NoteSaved += (s, ev) =>
			 {
				 Value = ((NoteActivity)s).TextNote;
				 _value.Text = Value;
				 _value.SetBackgroundColor(Android.Graphics.Color.ParseColor("#FAFAD2"));
			 };
			((Activity)_context).StartActivity(typeof(NoteActivity));
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
