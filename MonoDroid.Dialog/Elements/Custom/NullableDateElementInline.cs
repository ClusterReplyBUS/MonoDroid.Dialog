using System;
using Android.Graphics;
using Android.Widget;
using Java.Text;

namespace MonoDroid.Dialog
{
	public class NullableDateElementInline:StringElement
	{
		public object Tag { get; set; }
		public bool IsMandatory { get; set; }

		static string skey = "NullableDateTimeElementInline";
		public DateTime? DateValue;
		public event Action DateSelected;
		public event Action PickerClosed;
		public event Action PickerOpened;
		private InlineDateElement _inline_date_element = null;
		private bool _picker_present = false;
		private Color _defaultColor = Color.White;
		private DatePicker _mode ;

		//public NullableDateElementInline(string caption, DateTime? date) : this(caption, date) { }
		public NullableDateElementInline(string caption, DateTime? date, DatePicker mode)
			: base(caption)
		{
			DateValue = date;
			_mode = mode;
		//	Value = FormatDate(date);

		}

		public bool IsPickerOpen()
		{
			return _picker_present;
		}

		protected internal SimpleDateFormat fmt = new SimpleDateFormat()
		{
			
			//DateStyle = NSDateFormatterStyle.Medium
		};


		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				if (fmt != null)
				{
					fmt.Dispose();
					fmt = null;
				}
			}
		}

		//public virtual string FormatDate(DateTime? dt)
		//{
		//	//if (!dt.HasValue)
		//	//	return " ";

		//	//dt = GetDateWithKind(dt);
		//	//return fmt.ToString(dt.Value.ToNSDate());
		//}

		protected DateTime? GetDateWithKind(DateTime? dt)
		{
			if (!dt.HasValue)
				return dt;

			if (dt.Value.Kind == DateTimeKind.Unspecified)
				return DateTime.SpecifyKind(dt.Value, DateTimeKind.Local);

			return dt;
		}

		//public void ClosePickerIfOpen(DialogViewController dvc)
		//{
		//	if (_picker_present)
		//	{
		//		var index_path = this.IndexPath;
		//		var table_view = this.GetContainerTableView();

		//		Selected(dvc, table_view, index_path);
		//	}
		//}

		//public void SetDate(DateTime? date)
		//{
		//	this.DateValue = date;
		//	Value = FormatDate(date);
		//	var r = this.GetImmediateRootElement();
		//	r.Reload(this, UITableViewRowAnimation.None);
		//}

		public override void Selected()
		{
			base.Selected();
		}

		//private void TogglePicker(DialogViewController dvc, UITableView tableView, NSIndexPath path)
		//{
		//	var sectionAndIndex = GetMySectionAndIndex(dvc);
		//	if (sectionAndIndex.Key != null)
		//	{
		//		Section section = sectionAndIndex.Key;
		//		int index = sectionAndIndex.Value;

		//		var cell = tableView.CellAt(path);

		//		if (_picker_present)
		//		{
		//			// Remove the picker.
		//			cell.DetailTextLabel.TextColor = UIColor.Gray;
		//			section.Remove(_inline_date_element);
		//			_picker_present = false;
		//			if (PickerClosed != null)
		//				PickerClosed();
		//		}
		//		else
		//		{
		//			// Show the picker.
		//			cell.DetailTextLabel.TextColor = UIColor.Red;
		//			_inline_date_element = new InlineDateElement(DateValue, _mode);

		//			_inline_date_element.DateSelected += (DateTime? date) =>
		//			{
		//				this.DateValue = date;
		//				cell.DetailTextLabel.Text = FormatDate(date);
		//				Value = cell.DetailTextLabel.Text;
		//				cell.BackgroundColor = UIColor.FromRGB(1f, 1f, 0.8f);
		//				if (DateSelected != null)       // Fire our changed event.
		//					DateSelected();
		//			};

		//			_inline_date_element.ClearPressed += () =>
		//			{
		//				DateTime? null_date = null;
		//				DateValue = null_date;
		//				cell.DetailTextLabel.Text = " ";
		//				Value = cell.DetailTextLabel.Text;
		//				cell.DetailTextLabel.TextColor = UIColor.Gray;
		//				section.Remove(_inline_date_element);
		//				_picker_present = false;
		//				if (PickerClosed != null)
		//					PickerClosed();
		//				cell.BackgroundColor = _defaultColor ?? UIColor.White;
		//			};

		//			section.Insert(index + 1, UITableViewRowAnimation.Bottom, _inline_date_element);
		//			_picker_present = true;
		//			tableView.ScrollToRow(_inline_date_element.IndexPath, UITableViewScrollPosition.None, true);

		//			if (PickerOpened != null)
		//				PickerOpened();
		//		}
		//	}
		//}

		///// <summary>
		///// Locates this instance of this Element within a given DialogViewController.
		///// </summary>
		///// <returns>The Section instance and the index within that Section of this instance.</returns>
		///// <param name="dvc">Dvc.</param>
		//private KeyValuePair<Section, int> GetMySectionAndIndex(DialogViewController dvc)
		//{
		//	foreach (var section in dvc.Root)
		//	{
		//		for (int i = 0; i < section.Count; i++)
		//		{
		//			if (section[i] == this)
		//			{
		//				return new KeyValuePair<Section, int>(section, i);
		//			}
		//		}
		//	}
		//	return new KeyValuePair<Section, int>();
		//}

		//public override UITableViewCell GetCell(UITableView tv)
		//{
		//	var cell = base.GetCell(tv);
		//	cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
		//	cell.DetailTextLabel.Font = UIFont.SystemFontOfSize(17);

		//	cell.TextLabel.Font = UIFont.BoldSystemFontOfSize(17);
		//	if (this.IsMandatory)
		//	{
		//		//cell.TextLabel.TextColor = UIColor.Purple;
		//		cell.TextLabel.Text += "*";
		//	} /*else {
		//            //cell.TextLabel.TextColor = UIColor.Black;
		//        }*/
		//	if (DateValue.HasValue)
		//	{
		//		cell.BackgroundColor = UIColor.FromRGB(1f, 1f, 0.8f);
		//	}
		//	else
		//		cell.BackgroundColor = _defaultColor ?? UIColor.White;
		//	return cell;
		//} 


	}

	public class InlineDateElement : Element, IElementSizing
	{
		public InlineDateElement(DateTime? current_date, DatePicker mode)
				: base("")
		{
			//_current_date = current_date;
			//_date_picker = new UIDatePicker();
			//_date_picker.Mode = mode;
			//_picker_size = _date_picker.SizeThatFits(CGSize.Empty);
			//_cell_size = _picker_size;
			//_cell_size.Height += 30f; // Add a little bit for the clear button
		}

		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			return base.GetView(context, convertView, parent);
		}


		public float GetHeight(ListView lstv)
		{
			var width = (lstv.Width - 10) / 2;
			string reference = (Caption ?? string.Empty).Length > ("c" ?? string.Empty).Length ? Caption : "c";
			return (float)Math.Max(HeightForWidth(reference, width), 40F);
		}
	}
}
