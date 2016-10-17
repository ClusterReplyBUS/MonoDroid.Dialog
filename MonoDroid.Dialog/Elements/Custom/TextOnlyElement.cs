using System;
namespace MonoDroid.Dialog
{
	public partial class TextOnlyElement : ReadonlyElement
	{
		public TextOnlyElement(string caption)
			: base(caption, string.Empty)
		{

		}

		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			return base.GetView(context, convertView, parent);
		}

		//public override nfloat GetHeight(UITableView tableView, NSIndexPath indexPath)
		//{
		//	//return base.GetHeight(tableView, indexPath);
		//	//return LabelSize.Height + 30;
		//	var cell = GetCell(tableView);
		//	var height = HeightForWidth(cell.Frame.Width);
		//	//return Math.Max(cell.Frame.Height, height);
		//	return height;
		//}

	}
}