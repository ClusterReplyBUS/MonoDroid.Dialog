using System;
using System.Collections.Generic;
using Android.Graphics;

namespace MonoDroid.Dialog
{
	public class TaggedRootElement<ElementType> : RootElement
		where ElementType : ITaggedNodeElement
	{
		private string _backButtonLabel;
		private Dictionary<object, ElementType> _selectedChilds;
		private Color _defaultColor;

		public object Tag { get; set; }
		public bool IsMandatory { get; set; }

		public ElementType SelectedChild
		{
			get
			{
				if (SelectedChildren.ContainsKey("single"))
					return (ElementType)SelectedChildren["single"];
				return default(ElementType);
			}
			set
			{
				if (SelectedChildren.ContainsKey("single"))
					SelectedChildren["single"] = value;
				else
					SelectedChildren.Add("single", value);
			}
		}
		public Dictionary<object, ElementType> SelectedChildren
		{
			get
			{
				if (_selectedChilds == null)
					_selectedChilds = new Dictionary<object, ElementType>();
				return _selectedChilds;
			}
		}

		public TaggedRootElement(string caption, string backButtonLabel) : base(caption)
		{
			this.IsMandatory = false;
			this._backButtonLabel = backButtonLabel;
		}

		public TaggedRootElement(string caption, Group group, object tag, string backButtonLabel) : base(caption, group)
		{
			this.Tag = tag;
			this.IsMandatory = false;
			this._backButtonLabel = backButtonLabel;
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
