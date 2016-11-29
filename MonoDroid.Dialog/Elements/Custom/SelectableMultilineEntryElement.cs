﻿using System;
using Android.App;
using Android.Content;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class SelectableMultilineEntryElement : Element
	{
		public bool IsMandatory { get; set; }
		private TextView _caption;
		private TextView _value;
		public string Value { get; set; }
		private Context _context { get; set; }

		public SelectableMultilineEntryElement(string caption, string value, string saveLabel)
			: base(caption, (int)DroidResources.ElementLayout.dialog_note)
		{

		}

		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			_context = context;
			var view = DroidResources.LoadNoteElementLayout(context, convertView, parent,LayoutId,out _caption, out _value);
			if (view != null)
			{
				_caption.Text = Caption;
				if (string.IsNullOrEmpty(Value))
				{
					_value.Text = string.Empty;
				}

				if (_caption != null)
				{
					view.Click-= HandleEventHandler;
					view.Click += HandleEventHandler;
				}
			}
			return view;
		}

		void HandleEventHandler(object sender, EventArgs e)
		{
			NoteActivity.Instance.TextNote = Value;
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
