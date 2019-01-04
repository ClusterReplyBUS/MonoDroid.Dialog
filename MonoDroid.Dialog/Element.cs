using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Graphics;
namespace MonoDroid.Dialog
{
	public abstract class Element : Java.Lang.Object, IDisposable
	{
		/// <summary>
		///  Initializes the element with the given caption.
		/// </summary>
		/// <param name="caption">
		/// The caption.
		/// </param>
		public Element(string caption)
		{
			Caption = caption;
		}

		public Element(string caption, int layoutId)
		{
			Caption = caption;
			LayoutId = layoutId;
		}
        ///s.agostini
        public bool IsMandatory { get; set; }
        public bool IsMissing { get; set; }
		
		/// <summary>
		///  The caption to display for this given element
		/// </summary>
		public string Caption { get; set; }

		public int LayoutId { get; private set; }

		/// For sections this points to a RootElement, for every other object this points to a Section and it is null
		/// for the root RootElement.
		/// </remarks>
		public Element Parent { get; set; }

		/// <summary>
		/// Override for click the click event
		/// </summary>
		public Action Click { get; set; }

		/// <summary>
		/// Override for long click events, some elements use this for action
		/// </summary>
		public Action LongClick { get; set; }

		/// <summary>
		/// An Object that contains data about the element. The default is null.
		/// </summary>
		public Object Tag { get; set; }

		public void Dispose()
		{
			Dispose(true);
		}

		//protected virtual void Dispose(bool disposing) { }

		///// <summary>
		///// Returns a summary of the value represented by this object, suitable 
		///// for rendering as the result of a RootElement with child objects.
		///// </summary>
		///// <returns>
		///// The return value must be a short description of the value.
		///// </returns>
		public virtual string Summary()
		{
			return string.Empty;
		}

		public virtual float HeightForWidth(float width)
		{
			return HeightForWidth(Caption, width);
		}

		public virtual float HeightForWidth(string value, float width)
		{
			string h = string.Empty;
			if (value != null || value != string.Empty) { h = value; }
			Canvas c = new Canvas();
			//c.DrawRect(width, int.MaxValue, 0, 0, null);
			Paint paint = new Paint();
			int x = 0; int y = 0;
			c.DrawRect(x, y - paint.TextSize, x + paint.MeasureText(h), y, paint);
			//_labelSize = new NSString(Caption).
			//var h = new NSString(value ?? " ").
			//	 GetBoundingRect(
			//		 new CGSize(width, float.MaxValue),
			//		 NSStringDrawingOptions.UsesLineFragmentOrigin,
			//		 new UIStringAttributes() { Font = UIFont.BoldSystemFontOfSize(17) },
			//		 null).Height;
			return c.Height;
		}
		//static string cellkey = "xx";
		//protected virtual string CellKey
		//{
		//	get
		//	{
		//		return cellkey;
		//	}
		//}

		//public virtual ListView GetCell(ListView tv)
		//{
		//	//public virtual UITableViewCell GetCell(UITableView tv)
		//	//{
		//	//	return new UITableViewCell(UITableViewCellStyle.Default, CellKey);
		//	//}
		//	return new ListView(GetContext());
		//}

		//static protected void RemoveTag(ListView cell, int tag)
		//{
		//	//	static protected void RemoveTag(UITableViewCell cell, int tag)
		//	//{
	
		//	var viewToRemove = cell.FindViewWithTag(tag);
		//	if (viewToRemove != null) { }
		//		//viewToRemove.RemoveCallbacks();
		////}
		//}


		//public RootElement GetImmediateRootElement()
		//{
		//	var section = Parent as Section;
		//	if (section == null)
		//		section = this as Section;
		//	if (section == null)
		//		return null;
		//	return section.Parent as RootElement;
		//}

		//public ListView GetContainerTableView()
		//{
		//	var root = GetImmediateRootElement();
		//	if (root == null)
		//		return null;
		//	return root.TableView;
		//}

		//public ListView GetActiveCell()
		//{
		//	//public UITableViewCell GetActiveCell()
		//	//{
		//	//	var tv = GetContainerTableView();
		//	//	if (tv == null)
		//	//		return null;
		//	//	var path = IndexPath;
		//	//	if (path == null)
		//	//		return null;
		//	//	return tv.CellAt(path);
		//	//}
		//	var tv = GetContainerTableView();
		//	if (tv == null)
		//		return null;
		//	var path = IndexPath;
		//	if (path == null)
		//		return null;
		//	return (ListView)tv.GetChildAt(0); //tv.GetChildAt(path.Id);
		//}

		//public ViewGroup IndexPath
		//{
		//	get
		//	{
		//		var section = Parent as Section;
		//		if (section == null)
		//			return null;
		//		var root = section.Parent as RootElement;
		//		if (root == null)
		//			return null;

		//		int row = 0;
		//		foreach (var element in section.Elements)
		//		{
		//			if (element == this)
		//			{
		//				int nsect = 0;
		//				foreach (var sect in root.Sections)
		//				{
		//					if (section == sect)
		//					{
								
		//					//	return   IndexPath.FromRowSection(row, nsect);
		//					}
		//					nsect++;
		//				}
		//			}
		//			row++;
		//		}
		//		return null;
		//	}
		//	//get
		//	//{
		//	//	var section = Parent as Section;
		//	//	if (section == null)
		//	//		return null;
		//	//	var root = section.Parent as RootElement;
		//	//	if (root == null)
		//	//		return null;

		//	//	int row = 0;
		//	//	foreach (var element in section.Elements)
		//	//	{
		//	//		if (element == this)
		//	//		{
		//	//			int nsect = 0;
		//	//			foreach (var sect in root.Sections)
		//	//			{
		//	//				if (section == sect)
		//	//				{
		//	//					return NSIndexPath.FromRowSection(row, nsect);
		//	//				}
		//	//				nsect++;
		//	//			}
		//	//		}
		//	//		row++;
		//	//	}
		//	//	return null;
		//	//}
		//}

		public virtual bool Matches(string text)
		{
			if (Caption == null)
				return false;
			return Caption.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1;
		}

        /// <summary>
        /// Overriden my most derived classes, creates a view that creates a View with the contents for display
        /// </summary>
        /// <param name="context"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public virtual View GetView(Context context, View convertView, ViewGroup parent)
        {
			var view = LayoutId == 0 ? new View(context) : null;
            return view;
        }

        public virtual void Selected() {
			if (this.Click != null)
				this.Click();
		}
				
        

        public Context GetContext()
        {
            Element element = this;
            while (element.Parent != null)
                element = element.Parent;

            RootElement rootElement = element as RootElement;
            return rootElement == null ? null : rootElement.Context;
        }

    }



}