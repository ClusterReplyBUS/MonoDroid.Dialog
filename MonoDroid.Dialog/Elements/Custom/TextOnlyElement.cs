using System;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public partial class TextOnlyElement : Element
	{
		public TextOnlyElement(string caption)
			: base(caption,(int)DroidResources.ElementLayout.dialog_textonly)
		{

		}

		public override Android.Views.View GetView(Android.Content.Context context, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			TextView captionView;
			View view = DroidResources.LoadTextOnly(context, convertView, parent, LayoutId, out captionView);

			if (view != null)
			{
				captionView.Text = Caption;

                if (this.IsMissing)
                    captionView.SetTextColor(Color.ParseColor(Colors.MissingRed));

            }
            return view;
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