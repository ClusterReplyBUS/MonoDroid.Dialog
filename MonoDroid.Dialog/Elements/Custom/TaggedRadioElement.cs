using System;
using Android.Graphics;

namespace MonoDroid.Dialog
{
	public class TaggedRadioElement: RadioElement, ITaggedNodeElement
	{
		public bool IsBlank { get; set; }

		public Color _originalColor;

		public TaggedRadioElement(string caption) : base(caption)
		{
			
		}

		//public override void Selected()
		//{
		//	base.Selected();
		//}

	}
}
