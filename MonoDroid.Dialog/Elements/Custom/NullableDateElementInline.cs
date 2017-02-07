using System;
using Android.Graphics;
using Android.Widget;
using Java.Text;

namespace MonoDroid.Dialog
{
	public class NullableDateElementInline : DateElement
	{
		//public bool IsMandatory { get; set; }

		public NullableDateElementInline(string caption, DateTime? date)
			: base(caption, date)
		{
		}

		public static  DateFormat fmt = DateFormat.GetDateInstance(DateFormat.Medium);
		public virtual string FormatDate(DateTime? dt)
		{
			if (!dt.HasValue ||  string.IsNullOrWhiteSpace(dt.Value.ToString()))
				return " ";

			dt = GetDateWithKind(dt);

			Java.Util.Date d = new Java.Util.Date(DateTimeToUnixeTimeStamp(dt.Value));
			return fmt.Format(d);

		}


		protected DateTime? GetDateWithKind(DateTime? dt)
		{
			if (!dt.HasValue)
				return dt;

			if (dt.Value.Kind == DateTimeKind.Unspecified)
				return DateTime.SpecifyKind(dt.Value, DateTimeKind.Local);

			return dt;
		}

		public static long DateTimeToUnixeTimeStamp(DateTime dateTime)
		{
			return (long)(TimeZoneInfo.ConvertTimeToUtc(dateTime) -
					new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
		}

	}
}
