using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Java.Lang;


namespace MonoDroid.Dialog
{
	public class GridElement : Element
	{
		//public bool IsMandatory { get; set; }
		public List<GridHeader> Rows { get; set; }
		public List<GridHeader> Columns { get; set; }
		public GridAnswerType GridType { get; set; }
		public UserSource Source;
		public string Value { get; set; }

		public object ValueGrid { get; set; }
		//public object ValueGrid
		//{
		//	get { return Value; }

		//}

		private string _saveLabel;

		public GridElement(string caption, string saveLabel) : this(caption)
		{
			_saveLabel = saveLabel;
		}

		public GridElement(string caption) : base(caption, (int)DroidResources.ElementLayout.dialog_grid)
		//public GridElement(string caption) : base(caption, (int)DroidResources.ElementLayout.dialog_root)
		{

		}



		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			TextView _caption;
			TextView _value;
			var cell = DroidResources.LoadGridElementLayout(context, convertView, parent, LayoutId, out _caption, out _value);
			if (cell != null)
			{

                if (this.IsMissing)
                    _caption.SetTextColor(Color.ParseColor("#db0000"));

                if (IsMandatory && !string.IsNullOrWhiteSpace(Caption) && !Caption.EndsWith("*", StringComparison.InvariantCulture))
					Caption += "*";
				_caption.Text = Caption;
				if (this.Click == null)
				{
					cell.Click += (s, e) => this.Click();
					this.Click += () =>
					   {
						   if (Source == null)
						   {
							   Source = new UserSource()
							   {
								   GridType = this.GridType,
							   };
							   var lcol = new List<UserElement>();
							   lcol.Add(new UserElement(""));
							   foreach (var col in Columns)
							   {
								   lcol.Add(new UserElement(col.Text));
							   }
							   Source.Rows.Add(lcol);
							   foreach (var row in Rows)
							   {
								   var lrow = new List<UserElement>();
								   lrow.Add(new UserElement(row.Text));
								   foreach (var col in Columns)
								   {
									   lrow.Add(new UserElement(this.GridType, true, false)
									   {
										   AnswerId = row.AnswerId,
										   ColumnId = col.AnswerId
									   });
								   }
								   Source.Rows.Add(lrow);
							   }
						   }

						   GridActivity.Instance.Columns = Columns;
						   GridActivity.Instance.Rows = Rows;
						   GridActivity.Instance.TitleActivity = Caption;
						   GridActivity.Instance.GridType = GridType;
						   GridActivity.Instance.Source = Source;
						   GridActivity.Instance.SaveLabel = _saveLabel;
						   GridActivity.Instance.Save += (sender, e) =>
						   {
							   Source = e.Source;
							   _value.Text = GenerateText();
							   if (!string.IsNullOrEmpty(_value.Text))
							   {
								   _value.SetBackgroundColor(Android.Graphics.Color.ParseColor("#FAFAD2"));
							   }
						   };

						   ((Activity)context).StartActivity(typeof(GridActivity));
					   };
				}
			}
			_value.Text = GenerateText();
			if (!string.IsNullOrEmpty(_value.Text))
			{
				_value.SetBackgroundColor(Android.Graphics.Color.ParseColor("#FAFAD2"));

			}
			return cell;
		}

		public string GenerateText()
		{
			string text = "";
			if (Source == null)
				return string.Empty;
			foreach (var row in Source.Rows)
			{
				foreach (var el in row)
				{
					if (el.Checked == true)
					{
						if (text == "")
							text = this.Rows.SingleOrDefault(r => r.AnswerId == el.AnswerId).Text + "-" +
							 this.Columns.SingleOrDefault(r => r.AnswerId == el.ColumnId).Text +
						((GridType == GridElement.GridAnswerType.Text || GridType == GridElement.GridAnswerType.Number) ? ":" + el.Caption : "");

						else
							text += "\n" + this.Rows.SingleOrDefault(r => r.AnswerId == el.AnswerId).Text + "-" +
								this.Columns.SingleOrDefault(r => r.AnswerId == el.ColumnId).Text +
								((GridType == GridElement.GridAnswerType.Text || GridType == GridElement.GridAnswerType.Number) ? ":" + el.Caption : "");
					}
				}
			}
			return text;
		}

		public class GridHeader
		{
			public Guid AnswerId { get; set; }
			public string Text { get; set; }
		}

		public enum GridAnswerType
		{
			None = 0,
			Checkbox,
			Text,
			Number
		}
		public class UserSource  /*: UICollectionViewSource*/
		{
			public UserSource()
			{
				Rows = new List<List<UserElement>>();
			}
			public List<List<UserElement>> Rows { get; private set; }
			public GridAnswerType GridType { get; set; }
			public Single FontSize { get; set; }
		}



		public class UserElement
		{
			public UserElement(string caption, Action tapped)
			{
				Caption = caption;
				Tapped = tapped;
			}

			public UserElement(string caption)
			{
				Caption = caption;
				Type = GridAnswerType.None;
				Tappable = false;
				Checked = false;
			}

			public UserElement(GridAnswerType type)
			{
				Type = type;
				if (type == GridAnswerType.Checkbox)
					Caption = "☐";
				else
					Caption = "";
			}

			public UserElement(GridAnswerType type, bool tappable, bool check)
			{
				Type = type;
				if (type == GridAnswerType.Checkbox)
				{
					if (check)
						Caption = "☑";
					else
						Caption = "☐";
				}
				else
					Caption = "";
				Tappable = tappable;
				Checked = check;
			}

			public UserElement(GridAnswerType type, bool tappable, bool check, string caption)
			{
				Type = type;
				if (type == GridAnswerType.Checkbox)
				{
					if (check)
						Caption = "☑";
					else
						Caption = "☐";
				}
				else
					Caption = caption;
				Tappable = tappable;
				Checked = check;
			}

			public string Caption { get; set; }
			public bool Checked { get; set; }
			public bool Tappable { get; set; }
			public GridAnswerType Type { get; set; }
			public Guid AnswerId { get; set; }
			public Guid ColumnId { get; set; }
			public Action Tapped { get; set; }
			public View CellView { get; set; }
		}

		public class UserSourceEventArgs : EventArgs
		{
			public UserSource Source
			{
				get;
				set;
			}
			public UserSourceEventArgs(UserSource source)
			{
				Source = source;
			}
		}
	}
}

