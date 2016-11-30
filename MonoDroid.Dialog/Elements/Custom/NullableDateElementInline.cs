using System;
using Android.Graphics;
using Android.Widget;
using Java.Text;

namespace MonoDroid.Dialog
{
	public class NullableDateElementInline : DateElement
	{
		public bool IsMandatory { get; set; }

		public NullableDateElementInline(string caption, DateTime? date)
			: base(caption, date.HasValue ? date.Value : DateTime.Now)
		{
			Value = FormatDate(date);
		}

		protected internal DateFormat fmt = DateFormat.GetDateInstance(DateFormat.Medium);
		public virtual string FormatDate(DateTime? dt)
		{
			if (!dt.HasValue)
				return " ";

			dt = GetDateWithKind(dt);
			Java.Util.Date d = new Java.Util.Date(dt.Value.Millisecond);
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
	}
}
