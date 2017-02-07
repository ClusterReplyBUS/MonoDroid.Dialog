using System;
namespace MonoDroid.Dialog
{
	public class MultilineEntryElement: Element
	{
		public string Placeholder { get; set; }

		protected string val;
		//public bool IsMandatory { get; set; }

		public MultilineEntryElement(string caption, string value) : this(caption, "", value)
		{

		}
		public MultilineEntryElement(string caption, string placeholder, string value) : base(caption)
		{
			//Placeholder = placeholder;
			//Value = value;
		}

		public string Value
		{
			get
			{
				return val;
			}
			set
			{
				val = value;
				//if (entry != null)
				//	entry.Text = value;
			}
		}

		public bool Editable
		{
			get
			{
				return editable;
			}
			set
			{
				editable = value;
				//if (entry != null)
				//	entry.Editable = editable;
			}
		}

		protected bool editable;
		private int _numberOfRows;
		public int NumberOfRows
		{
			get { return _numberOfRows; }
			set
			{
				_numberOfRows = value;
				Height = _numberOfRows * 28;
			}
		}
		private float height = 112;
		public float Height
		{
			get
			{
				return height;
			}
			set
			{
				height = value;
			}
		}


	}
}
