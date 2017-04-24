using System;
using System.Globalization;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class DateTimeElement : Element
	{
		public string Value { get; set; }
		protected TextView label, value;
		public DateTime? DateValue
		{
			get
			{
				if (Value.Equals(" ") || Value == null)
					return null;
				string format = "dd MMM yyyy HH:mm";
				DateTime res;
				DateTime.TryParseExact(Value, format,
									   CultureInfo.InvariantCulture, DateTimeStyles.None, out res);
				return res;
			}
			set { Value = Format(value); }
		}

		public DateTimeElement(string caption, DateTime? date)
			: base(caption, (int)DroidResources.ElementLayout.dialog_date)
		{
			DateValue = date;
		}

		public DateTimeElement(string caption, DateTime date, int layoutId)
			: base(caption, layoutId)
		{
			DateValue = date;
		}
		public override Android.Views.View GetView(Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			//var view=base.GetView(context, convertView, parent);
			//_caption.Text = "CI";
			//_text.Text = "VI";
			//return view

			var view = DroidResources.LoadDateElementLayout(context, convertView, parent, LayoutId, out label, out value);
			if (view != null)
			{
				label.Text = Caption;
				value.Text = Value;
				if (!string.IsNullOrWhiteSpace(value.Text))
				{
					value.SetBackgroundColor(Color.ParseColor("#FAFAD2"));

				}
				if (this.IsMandatory && Caption != null && !Caption.EndsWith("*", StringComparison.InvariantCulture))
				{
					this.Caption += "*";
					label.Text = Caption;
				}
			}
			return view;
		}

		public virtual string Format(DateTime? dt)
		{
			//return dt.ToShortDateString() + " " + dt.ToShortTimeString();
			if (dt.HasValue)
				return NullableDateElementInline.fmt.Format(new Java.Util.Date(NullableDateElementInline.DateTimeToUnixeTimeStamp(dt.Value))) + dt.Value.ToString(" HH:mm");
			return " ";
		}



	}

	public class DateElement : DateTimeElement
	{

		public DateElement(string caption, DateTime? date)
			: base(caption, date)
		{

			this.Click = delegate { EditDate(); };
		}

		public DateElement(string caption, DateTime date, int layoutId)
			: base(caption, date, layoutId)
		{
			this.Click = delegate { EditDate(); };
		}

		// the event received when the user "sets" the date in the dialog
		void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			DateTime? current = DateValue;
			if (!current.HasValue)
				current = DateTime.Now;
				DateValue = new DateTime(e.Date.Year, e.Date.Month, e.Date.Day, current.Value.Hour, current.Value.Minute, 0);
				//value.Text = NullableDateElementInline.fmt.Format(new Java.Util.Date(NullableDateElementInline.DateTimeToUnixeTimeStamp(DateValue)));
				EditTime();

		}

		private void EditDate()
		{
			Context context = GetContext();
			if (context == null)
			{
				Android.Util.Log.Warn("DateElement", "No Context for Edit");
				return;
			}
			DateTime? val = DateValue;
			if (!val.HasValue)
				val = DateTime.Now;
			new DatePickerDialog(context, OnDateSet, val.Value.Year, val.Value.Month - 1, val.Value.Day).Show();
		}
		void OnTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
		{
			DateTime? time = DateValue;
			if (!time.HasValue)
				time = DateTime.Now;

			DateValue = new DateTime(time.Value.Year, time.Value.Month, time.Value.Day, e.HourOfDay, e.Minute, 0);
			value.Text = Format(DateValue);
		}

		private void EditTime()
		{
			Context context = GetContext();
			if (context == null)
			{
				Android.Util.Log.Warn("TimeElement", "No Context for Edit");
				return;
			}
			DateTime? val = DateValue;
			// TODO: get the current time setting for thge 24 hour clock
			if (!val.HasValue)
				val = DateTime.Now;
				new TimePickerDialog(context, OnTimeSet, val.Value.Hour, val.Value.Minute, true).Show();

		}
	}

	public class TimeElement : DateTimeElement
	{
		public TimeElement(string caption, DateTime date)
			: base(caption, date)
		{
			this.Click = delegate { EditDate(); };
		}

		public TimeElement(string caption, DateTime date, int layoutId)
			: base(caption, date, layoutId)
		{
			this.Click = delegate { EditDate(); };
		}

		public override string Format(DateTime? dt)
		{
			if (dt.HasValue)
				return dt.Value.ToShortTimeString();
			return null;
		}

		// the event received when the user "sets" the date in the dialog
		void OnDateSet(object sender, TimePickerDialog.TimeSetEventArgs e)
		{
			DateTime? current = DateValue;
			if (current.HasValue)
				DateValue = new DateTime(current.Value.Year, current.Value.Month, current.Value.Day, e.HourOfDay, e.Minute, 0);

		}

		private void EditDate()
		{
			Context context = GetContext();
			if (context == null)
			{
				Android.Util.Log.Warn("TimeElement", "No Context for Edit");
				return;
			}
			DateTime? val = DateValue;
			// TODO: get the current time setting for thge 24 hour clock
			if (val.HasValue)
				new TimePickerDialog(context, OnDateSet, val.Value.Hour, val.Value.Minute, false).Show();
		}


	}
}