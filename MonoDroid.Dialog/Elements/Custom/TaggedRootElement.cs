using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;

namespace MonoDroid.Dialog
{
	public class TaggedRootElement<ElementType> : RootElement, IDialogInterfaceOnClickListener
		where ElementType : ITaggedNodeElement
	{
		private string _backButtonLabel;
		private Dictionary<object, ElementType> _selectedChilds;
		private Color _defaultColor;

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
			var view = base.GetView(context, convertView, parent);
			//if (!(_group is RadioGroup))
			//{
			//	_showValue = SelectedChildren.Count.ToString();
			//}
			
			return view;
		}

		protected override void OnChildSelected(int which)
		{
			var radio = _group as RadioGroup;
			if (radio != null)
			{
				var e = GetSelectedRadioElement(radio);
				e.OnChildSelected();
			}
			else
			{
				var e = GetSelectedCheckBoxElement(which);
				e.OnChildSelected();
				_showValue = SelectedChildren.Count.ToString();
			}


		}
		//public override void Selected()
		//{
		//	base.Selected();
		//}
		private RadioElement GetSelectedRadioElement(RadioGroup radio)
		{
			int selected = radio.Selected;
			int current = 0;
			foreach (var s in Sections)
			{
				foreach (var e in s.Elements)
				{
					if (!(e is RadioElement))
						continue;

					if (current == selected)
						return e as RadioElement;

					current++;
				}
			}
			return null;
		}

		private CheckboxElement GetSelectedCheckBoxElement(int which)
		{
			int current = 0;
			foreach (var s in Sections)
			{
				foreach (var e in s.Elements)
				{
					if (!(e is CheckboxElement))
						continue;

					if (current == which)
						return e as CheckboxElement;
					current++;
				}
			}
			return null;
		}

	}
}
