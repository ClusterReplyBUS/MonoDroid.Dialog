using System;
using System.Collections.Generic;

namespace MonoDroid.Dialog
{
	public class TwoStateElement:Element
	{
		public object Tag { get; set; }

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

				bool emit = (val == null && value != null) || (val != null && value == null) || val.Id != value.Id;
				val = value;
				if (emit && ValueChanged != null)
					ValueChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler ValueChanged;

		public TwoStateElement(string caption, TwoStateChoice firstChoice, TwoStateChoice secondChoice, bool firstIsDefault) : base(caption)
		{
			this.choices = new List<TwoStateChoice>() { firstChoice, secondChoice };
			val = firstIsDefault ? firstChoice : secondChoice;
		}

		static string bkey = "TwoStateElement";
		//UISegmentedControl sc;
		//protected override string CellKey
		//{
		//	get
		//	{
		//		return bkey;
		//	}
		//}

		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			return base.GetView(context, convertView, parent);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}

	public class TwoStateChoice
	{
		public Guid Id { get; set; }

		public string Text { get; set; }
	}
}
