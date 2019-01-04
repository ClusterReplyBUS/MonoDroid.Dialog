using System;

using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class ButtonElement : StringElement
	{
		public ButtonElement (string caption, Action tapped)
            : base(caption, (int)DroidResources.ElementLayout.dialog_button)
		{
			this.Click = tapped;
		}

		public override View GetView (Context context, View convertView, ViewGroup parent)
		{
			Button button;
			var view = DroidResources.LoadButtonLayout (context, convertView, parent, LayoutId, out button);
			if (view != null) {

                if (this.IsMissing)
                    _caption.SetTextColor(Color.ParseColor(Colors.MissingRed));

                button.Text = Caption;
				if (Click != null)
				{
					button.Click += delegate { Click(); };
					view.Click += delegate { Click(); };
				}
			}
			
			return view;
		}

		public override string Summary ()
		{
			return Caption;
		}

	}
}