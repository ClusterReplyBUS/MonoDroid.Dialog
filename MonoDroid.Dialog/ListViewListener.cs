using System;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class ListViewListener: AbsListView.IRecyclerListener
	{
		public ListViewListener()
		{
		}

		public IntPtr Handle
		{
			get
			{
				
				throw new NotImplementedException();
			}
		}

		public void Dispose()
		{
			
		}

		public void OnMovedToScrapHeap(View view)
		{
			if (view.HasFocus)
			{
				view.ClearFocus(); //we can put it inside the second if as well, but it makes sense to do it to all scraped views
								   //Optional: also hide keyboard in that case
				if (view is EditText) {
					InputMethodManager imm = (InputMethodManager)view.Context.GetSystemService(Android.Content.Context.InputMethodService);
					imm.HideSoftInputFromWindow(view.WindowToken, 0);
				}
			}
		}
	}
}
