using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Java.Lang;


namespace MonoDroid.Dialog
{
	public class GridElement : RootElement
	{
		public bool Mandatory { get; set; }

		public string Value { get; set; }

		public List<GridHeader> Rows { get; set; }
		public List<GridHeader> Columns { get; set; }
		public GridAnswerType GridType { get; set; }
		public UserSource userSource;

		static string hkey = "GridElement";

		private string _saveLabel;



		public GridElement(string caption, string saveLabel) : this(caption)
		{
			_saveLabel = saveLabel;
		}

		public GridElement(string caption) : base(caption)
		{
			
		}

		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			this.Click += () =>
			  {
				  GridActivity.Instance.Columns = Columns;
				  GridActivity.Instance.Rows = Rows;
				  GridActivity.Instance.Title = Caption;
				((Activity)context).StartActivity(typeof(GridActivity));
			  };
			return base.GetView(context, convertView, parent);
		}

		//class GridViewController : UIViewController
		//{
		//	public GridViewController(GridElement container) : base()
		//	{
		//		this.View.BackgroundColor = UIColor.White;
		//	}

		//	public override void ViewWillDisappear(bool animated)
		//	{
		//		base.ViewWillDisappear(animated);
		//	}

		//	public bool Autorotate { get; set; }

		//	//			public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		//	//			{
		//	//				return Autorotate;
		//	//			}
		//}

		//public override void Selected()
		//{
		//	base.Selected();
		//}

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
		public class UserSource : BaseAdapter  /*: UICollectionViewSource*/
		{
			public UserSource()
			{
				Rows = new List<List<UserElement>>();
			}

			public List<List<UserElement>> Rows { get; private set; }
			public GridAnswerType GridType { get; set; }
			public Single FontSize { get; set; }

			public override int Count
			{
				get
				{
					return Rows.Count;
				}
			}

			public override Java.Lang.Object GetItem(int position)
			{
				throw new NotImplementedException();
			}

			public override long GetItemId(int position)
			{
				throw new NotImplementedException();
			}

			public override View GetView(int position, View convertView, ViewGroup parent)
			{
				throw new NotImplementedException();
			}
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
		}
	}
}
