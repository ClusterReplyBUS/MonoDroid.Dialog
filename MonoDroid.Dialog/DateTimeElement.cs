using System;
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
        public DateTime DateValue
        {
			get {
				if (Value.Equals(" ") || Value==null)
					return DateTime.Now;
				return DateTime.Parse(Value);
						
			}
            set { Value = Format(value); }
        }

        public DateTimeElement(string caption, DateTime date)
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
				if (!string.IsNullOrWhiteSpace(value.Text) )
				{
					value.SetBackgroundColor(Color.ParseColor("#FAFAD2"));
					
				}
			}
			return view;
		}
        
        public virtual string Format(DateTime dt)
        {
            //return dt.ToShortDateString() + " " + dt.ToShortTimeString();
			return NullableDateElementInline.fmt.Format(new Java.Util.Date(NullableDateElementInline.DateTimeToUnixeTimeStamp(dt))) + dt.ToString(" HH:mm");
        }



    }

    public class DateElement : DateTimeElement
    {
		
        public DateElement(string caption, DateTime date)
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
            DateTime current = DateValue;
            DateValue = new DateTime(e.Date.Year, e.Date.Month, e.Date.Day, current.Hour, current.Minute, 0);
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
            DateTime val = DateValue;
			new DatePickerDialog(context, OnDateSet, val.Year, val.Month - 1, val.Day).Show();	
		}
		void OnTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
		{
			DateTime time = DateValue;
			DateValue = new DateTime(time.Year, time.Month, time.Day, e.HourOfDay, e.Minute, 0);
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
			DateTime val = DateValue;
			// TODO: get the current time setting for thge 24 hour clock
			new TimePickerDialog(context, OnTimeSet, val.Hour, val.Minute, true).Show();

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

        public override string Format(DateTime dt)
        {
            return dt.ToShortTimeString();
        }

        // the event received when the user "sets" the date in the dialog
        void OnDateSet(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            DateTime current = DateValue;
            DateValue = new DateTime(current.Year, current.Month, current.Day, e.HourOfDay, e.Minute, 0);

		}

        private void EditDate()
        {
            Context context = GetContext();
            if (context == null)
            {
                Android.Util.Log.Warn("TimeElement", "No Context for Edit");
                return;
            }
            DateTime val = DateValue;
            // TODO: get the current time setting for thge 24 hour clock
            new TimePickerDialog(context, OnDateSet, val.Hour, val.Minute, false).Show();
        }


    }
}