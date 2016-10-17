using System;
using Android.Graphics;

namespace MonoDroid.Dialog
{
	public class RightAlignEntryElement: EntryElement, IElementSizing
	{
		#region PRIVATE MEMBERS
		private string _placeholder;
		private Color _defaultColor = Color.White;
		//private UIColor _defaultColor = UIColor.FromRGB(247, 247, 247);
		private string _cellKey = "RightAlignEntryElement";
		#endregion

		#region PUBLIC PROPERTIES
		public object Tag { get; set; }

		public bool IsMandatory { get; set; }

		//public bool IsPassword { get; set; }
		#endregion

		public float GetHeight()
		{
			throw new NotImplementedException();
		}

		#region CTOR
		public RightAlignEntryElement(string caption, string placeholder, string value) : base(caption, placeholder, value)
		{
			throw new NotImplementedException();
			//base.Changed += HandleBaseChanged;
		}

		//public RightAlignEntryElement(string caption, string placeholder, string value, bool isPassword) : base(caption, placeholder, value, isPassword)
		//{
		//	this._placeholder = placeholder;
		//	//this.IsPassword = isPassword;
		//	this.IsMandatory = false;
		//	this.TextAlignment = UITextAlignment.Right;
		//	this.ClearButtonMode = UITextFieldViewMode.WhileEditing;

		//	base.Changed += HandleBaseChanged;
		//}
		#endregion

		//#region PRIVATE METHODS
		//private void HandleBaseChanged(object sender, EventArgs e)
		//{
		//	var cell = this.GetActiveCell();
		//	if (cell != null)
		//	{
		//		if (!string.IsNullOrEmpty(this.Value))
		//		{
		//			cell.BackgroundColor = UIColor.FromRGB(1f, 1f, 0.8f);
		//		}
		//		else
		//			cell.BackgroundColor = _defaultColor;
		//	}
		//}

		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			return base.GetView(context, convertView, parent);
		}
	}
}
