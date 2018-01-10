using System;
namespace MonoDroid.Dialog
{
	public class ProductOfInterestRadioElement: TaggedRadioElement
	{
		public string ProductGroup { get; set; }

		public ProductOfInterestRadioElement(string caption) : base(caption)
		{
		}

		public ProductOfInterestRadioElement(string caption, string productGroup) : base(caption)
		{
			ProductGroup = productGroup;
		}
	}
}
