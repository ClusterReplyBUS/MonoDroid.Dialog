using System;
using System.Collections.Generic;
using System.Linq;

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
				bool emit = (val == null && value != null) || (val != null && value == null) || val.Id != value.Id;
				val = value;
				if (emit && ValueChanged != null)
					ValueChanged(this, EventArgs.Empty);
				//bool emit = (val == null && value != null) || (val != null && value == null) || val.Id != value.Id;
				//val = value;
			}
		}

		public event EventHandler ValueChanged;

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

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}


		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			if (this.IsMandatory && Caption != null && !Caption.EndsWith("*", StringComparison.InvariantCulture))
			{
				Caption += "*";
			}
			var view = base.GetView(context, convertView, parent);
			_toggleButton.SetHighlightColor(Android.Graphics.Color.ParseColor(Colors.PrimaryColor));
			_toggleButton.TextOn = choices[0].Text;
			_toggleButton.TextOff = choices[1].Text;
			_toggleButton.Text = Value.Text;
			_toggleButton.Selected = false;
			_toggleButton.CheckedChange += _toggleButton_CheckedChange;
			return view;
		}

		void _toggleButton_CheckedChange(object sender, Android.Widget.CompoundButton.CheckedChangeEventArgs e)
		{
			Value = e.IsChecked ? choices[0] : choices[1];
		}

		//void _toggleButton_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
		//{
		//	var choice = choices.Where(c => c.Id == val.Id).FirstOrDefault();
		//	if (choice != null)
		//	{
		//		var index = choices.IndexOf(choice);
		//		val = choices[index];
		//		ValueChanged(sender, EventArgs.Empty);
		//	}
		//	//var choice=choices.FindIndex(z => z.Id == val.Id);
		//}

	}

	public class TwoStateChoice
	{
		public Guid Id { get; set; }

		public string Text { get; set; }
	}
}
