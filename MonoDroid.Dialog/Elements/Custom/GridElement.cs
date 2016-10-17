using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace MonoDroid.Dialog
{
	public class GridElement : Element, IElementSizing
	{
		public bool Mandatory { get; set; }

		public object Value { get; set; }

		public List<GridHeader> Rows { get; set; }
		public List<GridHeader> Columns { get; set; }
		public GridAnswerType GridType { get; set; }
		public UserSource userSource;

		static string hkey = "GridElement";

		private string _saveLabel;

		public object Tag { get; set; }

		public GridElement(string caption, string saveLabel) : this(caption)
		{
			_saveLabel = saveLabel;
		}
		public GridElement(string caption) : base(caption)
		{
		}

		//protected override NSString CellKey
		//{
		//	get
		//	{
		//		return hkey;
		//	}
		//}

		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			return base.GetView(context, convertView, parent);
		}

		public float GetHeight()
		{
			throw new NotImplementedException();
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

		public override void Selected()
		{
			base.Selected();
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
		public class UserSource: BaseAdapter  /*: UICollectionViewSource*/
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
					throw new NotImplementedException();
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


			//public override Int32 NumberOfSections(UICollectionView collectionView)
			//{
			//	return Rows.Count;
			//}

			//public override Int32 GetItemsCount(UICollectionView collectionView, Int32 section)
			//{
			//	return Rows[section].Count;
			//}

			//public override nint NumberOfSections(UICollectionView collectionView)
			//{
			//	return Rows.Count;
			//}

			//public override nint GetItemsCount(UICollectionView collectionView, nint section)
			//{
			//	return Rows[(int)section].Count;
			//}


			//public override Boolean ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
			//{
			//	return true;
			//}

			////			public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
			////			{
			////				var cell = (UserCell) collectionView.CellForItem(indexPath);
			////			}

			//public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
			//{
			//	var cell = (UserCell)collectionView.CellForItem(indexPath);

			//	UserElement row = Rows[indexPath.Section][indexPath.Row];
			//	if (row.Tappable)
			//	{
			//		if (GridType == GridAnswerType.Checkbox)
			//		{
			//			row.Checked = row.Checked ? false : true;
			//			row.Caption = row.Checked ? "☑" : "☐";
			//		}
			//		else if (GridType == GridAnswerType.Text)
			//		{
			//			row.Checked = true;
			//			row.Caption = "risposta";
			//		}
			//		cell.UpdateRow(row, FontSize);
			//	}
			//	//row.Tapped.Invoke();
			//}


			//public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
			//{
			//	var cell = (UserCell)collectionView.DequeueReusableCell(UserCell.CellID, indexPath);

			//	UserElement row = Rows[indexPath.Section][indexPath.Row];

			//	cell.UpdateRow(row, FontSize);

			//	return cell;
			//}
		}
		public class UserElement
		{
			//			public UserElement(String caption, NSAction tapped)
			//			{
			//				Caption = caption;
			//				Tapped = tapped;
			//			}

			public UserElement(string caption)
			{
				Caption = caption;
				Type = GridAnswerType.None;
				Tappable = false;
				Checked = false;
			}

			//			public UserElement(GridAnswerType type)
			//			{
			//				Type = type;
			//				if (type == GridAnswerType.Checkbox)
			//					Caption = "☐";
			//				else
			//					Caption = "";
			//			}

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

		//			public NSAction Tapped { get; set; }
	}
	}
}
