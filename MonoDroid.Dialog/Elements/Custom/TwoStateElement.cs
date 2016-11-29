using System;
using System.Collections.Generic;

namespace MonoDroid.Dialog
{
	public class TwoStateElement : BooleanElement
	//public class TwoStateElement:Element
	{
		//public object Tag { get; set; }

		public bool IsMandatory { get; set; }

		TwoStateChoice val;
		List<TwoStateChoice> choices;

		public virtual TwoStateChoice Value
		{
			get
			{
				return val;
			}
			set
			{

				//bool emit = (val == null && value != null) || (val != null && value == null) || val.Id != value.Id;
				val = value;
			}
		}

		//public event EventHandler ValueChanged;

		public TwoStateElement(string caption, TwoStateChoice firstChoice, TwoStateChoice secondChoice, bool firstIsDefault) : base(caption, firstIsDefault)
		{
			this.choices = new List<TwoStateChoice>() { firstChoice, secondChoice };
			val = firstIsDefault ? firstChoice : secondChoice;
		}

		//static string bkey = "TwoStateElement";
		////UISegmentedControl sc;
		////protected override string CellKey
		////{
		////	get
		////	{
		////		return bkey;
		////	}
		////}

		//public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		//{
		//	if (this.IsMandatory && Caption != null && !Caption.EndsWith("*", StringComparison.InvariantCulture))
		//	{
		//		Caption += "*";
		//	}
		//	return base.GetView(context, convertView, parent);
		//}

		//protected override void Dispose(bool disposing)
		//{
		//	base.Dispose(disposing);
		//}
		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			if (this.IsMandatory && Caption != null && !Caption.EndsWith("*", StringComparison.InvariantCulture))
			{
				Caption += "*";
			}
			var view = base.GetView(context, convertView, parent);
			_toggleButton.TextOn = choices[0].Text;
			_toggleButton.TextOff = choices[1].Text;
			return view;
		}
	}

	public class TwoStateChoice
	{
		public Guid Id { get; set; }

		public string Text { get; set; }
	}
}
