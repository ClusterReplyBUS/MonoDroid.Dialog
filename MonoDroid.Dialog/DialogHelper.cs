using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace MonoDroid.Dialog
{
    public class DialogHelper
	{
		private Context context;
		private RootElement formLayer;

		//public event Action<Section, Element> ElementClick;
		//public event Action<Section, Element> ElementLongClick;

		public RootElement Root { get; set; }

		private DialogAdapter DialogAdapter { get; set; }

		public DialogHelper(Context context, ListView dialogView, RootElement root)
		{
			this.Root = root;
			this.Root.Context = context;

			dialogView.Adapter = this.DialogAdapter = new DialogAdapter(context, this.Root);
			dialogView.ItemClick += ListView_ItemClick;
			dialogView.ItemLongClick += ListView_ItemLongClick; ;
			dialogView.Tag = root;
			dialogView.Recycler += DialogView_Recycler;
      
            dialogView.ScrollStateChanged += (sender, e) => {
                if(e.ScrollState==0){
                    InputMethodManager imm = (InputMethodManager)e.View.Context.GetSystemService(Android.Content.Context.InputMethodService);
                    imm.HideSoftInputFromWindow(e.View.WindowToken, 0);
                }
            };
            //InputMethodManager inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            //inputMethodManager.ShowSoftInput(dialogView, ShowFlags.Forced);
            //inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
		}

		void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
		{
			var elem = this.DialogAdapter.ElementAtIndex(e.Position);
			if (elem != null && elem.LongClick != null)
			{
				elem.LongClick();
			}
		}

		void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{

			var elem = this.DialogAdapter.ElementAtIndex(e.Position);

			if (elem != null)
			{
				elem.Selected();
			}
            //InputMethodManager inputMethodManager = e.View.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            //inputMethodManager.ShowSoftInput(e.View, ShowFlags.Forced);
            //inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
		}


        public void ReloadData()
		{
			if (Root == null)
			{
				return;
			}
			this.DialogAdapter.ReloadData();
		}

		void DialogView_Recycler(object sender, AbsListView.RecyclerEventArgs e)
		{
			if (e.View.HasFocus)
			{
				e.View.ClearFocus();
				//we can put it inside the second if as well, but it makes sense to do it to all scraped views
				//Optional: also hide keyboard in that case
				if (e.View is EditText)
				{
					InputMethodManager imm = (InputMethodManager)e.View.Context.GetSystemService(Android.Content.Context.InputMethodService);
					imm.HideSoftInputFromWindow(e.View.WindowToken, 0);
				}
			}
		}




	}
}