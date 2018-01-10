
using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
	[Activity(Label = "GridActivity")]

	public class GridActivity : Activity
	{
		private static volatile GridActivity _instance;

		public GridActivity() : base()
		{
			if (_instance != null)
			{
				this.Rows = _instance.Rows;
				this.Columns = _instance.Columns;
				this.GridType = _instance.GridType;
				this.TitleActivity = _instance.TitleActivity;
				this.Source = _instance.Source;
				this.SaveLabel = _instance.SaveLabel;
				this.Save = _instance.Save;
			}
			_instance = this;
		}
		public static GridActivity Instance
		{
			get
			{
				if (_instance == null)
					_instance = new GridActivity();
				return _instance;
			}
		}

		public List<GridElement.GridHeader> Rows { get; set; }
		public List<GridElement.GridHeader> Columns { get; set; }
		public GridElement.GridAnswerType GridType { get; set; }
		public string TitleActivity { get; set; }
		public GridElement.UserSource Source { get; set; }
		public string SaveLabel { get; set; }


		public override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();
			Window.SetTitle(TitleActivity);
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			ActionBar actionBar = ActionBar;
			actionBar.SetBackgroundDrawable(new ColorDrawable(Color.ParseColor(Colors.PrimaryColor))); // set your desired color

			Point size = new Point();
			WindowManager.DefaultDisplay.GetSize(size);
			int halfScreenWidth = size.X / 2;
			int quarterScreenWidth = size.X / 4;
			int quarterScreenHeight = size.Y / 8;
			List<GridLayout.Spec> gridRows = new List<GridLayout.Spec>();
			for (int i = 0; i < Rows.Count + 1; i++)
				gridRows.Add(GridLayout.InvokeSpec(i,  GridLayout.Center));
			List<GridLayout.Spec> gridCols = new List<GridLayout.Spec>();
			for (int j = 0; j < Columns.Count + 1; j++)
				gridCols.Add(GridLayout.InvokeSpec(j));

			GridLayout gridLayout = new GridLayout(this);
			gridLayout.ColumnCount = Columns.Count + 1;
			gridLayout.RowCount = Rows.Count + 1;

			for (int i = 0; i < Columns.Count + 1; i++)
				for (int j = 0; j < Rows.Count + 1; j++)
				{
					View intView = null;
					//GridLayout.LayoutParams elem = new GridLayout.LayoutParams(gridRows[j], gridCols[i]);
					GridLayout.LayoutParams elem = new GridLayout.LayoutParams(gridRows[j], gridCols[i]);
					elem.SetMargins(0, 0, 10, 0);
					elem.SetGravity(GravityFlags.CenterHorizontal);
					if (i == 0 && j == 0)
					{
						intView = new TextView(this);
						intView.LayoutParameters = elem;
						//elem.Width = quarterScreenWidth;
						//elem.Height = quarterScreenHeight;
						//intView.SetBackgroundColor(Color.Blue);
					}
					else if (i == 0)
					{
						elem.SetMargins(5, 0, 0, 0);
						intView = new TextView(this);
						intView.LayoutParameters = elem;
						//intView.SetBackgroundColor(Color.Blue);
						//elem.Width = quarterScreenWidth;
						//elem.Height = quarterScreenHeight;
						intView.Tag = Rows[j - 1].AnswerId.ToString();
						((TextView)intView).Text = Rows[j - 1].Text;
						((TextView)intView).SetTextColor(Color.Black);
						intView.TextAlignment = TextAlignment.Center;
						((TextView)intView).Gravity = GravityFlags.Center;
					}
					else if (j == 0)
					{
						intView = new TextView(this);
						intView.LayoutParameters = elem;
						intView.SetMinimumWidth(quarterScreenWidth);
						//elem.Width = quarterScreenWidth/2;
						//elem.Height = quarterScreenHeight/2;
						intView.TextAlignment = TextAlignment.Center;
						((TextView)intView).Gravity = GravityFlags.Center;
						//intView.SetBackgroundColor(Color.Yellow);
						intView.Tag = Columns[i - 1].AnswerId.ToString();
						((TextView)intView).Text = Columns[i - 1].Text;
						((TextView)intView).SetTextColor(Color.Black);
					}
					else
					{
						elem.SetMargins(10, 0, 0, 0);
						if (GridType == GridElement.GridAnswerType.Checkbox)
						{
							intView = new CheckBox(this);
							intView.TextAlignment = TextAlignment.Center;
							((CheckBox)intView).Gravity = GravityFlags.Center;
							intView.LayoutParameters = elem;
							elem.Width = quarterScreenWidth;
							elem.Height = quarterScreenHeight;
							((CheckBox)intView).SetTextColor(Color.Black);
							((CheckBox)intView).Enabled = true;
							//if (Source != null && Source.Rows != null && Source.Rows[i - 1] != null && Source.Rows[j - 1][i - 1] != null && !string.IsNullOrEmpty(Source.Rows[i - 1][j - 1].Caption))
							if (Source != null && Source.Rows != null && Source.Rows[j] != null && Source.Rows[j][i] != null)
								((CheckBox)intView).Checked = Source.Rows[j][i].Checked;
							else
								((CheckBox)intView).Checked = false;
						}
						else if (GridType == GridElement.GridAnswerType.Text || GridType == GridElement.GridAnswerType.Number)
						{
							intView = new EditText(this);
							((EditText)intView).Gravity = GravityFlags.Center;
							intView.LayoutParameters = elem;
							LayerDrawable bottomBorder = getBorders(
							Color.White, // Background color
							Color.ParseColor(Colors.PrimaryColor), // Border color
							0, // Left border in pixels
							0, // Top border in pixels
							0, // Right border in pixels
							1 // Bottom border in pixels
							);
							elem.Width = quarterScreenWidth;
							elem.Height = quarterScreenHeight;
							((EditText)intView).SetTextColor(Color.Black);
							//((EditText)intView).SetBackgroundColor(Color.White);

							//if (Source != null && Source.Rows != null && Source.Rows[i - 1] != null && Source.Rows[j - 1][i - 1] != null && !string.IsNullOrEmpty(Source.Rows[i - 1][j - 1].Caption))
							if (Source != null && Source.Rows != null && Source.Rows[j] != null && Source.Rows[j][i] != null)
							{
								((EditText)intView).Text = Source.Rows[j][i].Caption;
							}
							else
							{
								((EditText)intView).Text = string.Empty;
							}
							if (GridType == GridElement.GridAnswerType.Number)
								((EditText)intView).InputType = Android.Text.InputTypes.ClassPhone;

							intView.Background = bottomBorder;

							//((EditText)intView).SetBackgroundDrawable(drawable);
						}
						Source.Rows[j][i].CellView = intView;

					}
					gridLayout.AddView(intView, elem);
				}

			gridLayout.SetBackgroundColor(Color.White);
			gridLayout.SetMinimumWidth(size.X);
			gridLayout.SetMinimumHeight(size.Y);
			var sv = new ScrollView(this);
			var hscroll = new HorizontalScrollView(this);
			hscroll.SetMinimumWidth(size.X);
			hscroll.SetMinimumHeight(size.Y);
			sv.HorizontalScrollBarEnabled = true;
			sv.VerticalScrollBarEnabled = true;
			RelativeLayout relative = new RelativeLayout(this);
			relative.CanScrollHorizontally(size.X);
			//relative.SetMinimumWidth(size.X);
			//linear.SetMinimumHeight(size.Y);
			relative.AddView(gridLayout);
			hscroll.AddView(relative);
			sv.AddView(hscroll);

			this.SetContentView(sv);
		}

		void CheckGridForSave()
		{
			for (int i = 1; i < Columns.Count + 1; i++)
				for (int j = 1; j < Rows.Count + 1; j++)
				{
					GridElement.UserElement row = Source.Rows[j][i];
					/*if (row.Tappable)
					   {
						   if (GridType == GridElement.GridAnswerType.Checkbox)
						   {
							   row.Checked = row.Checked ? false : true;
							   row.Caption = row.Checked ? "☑" : "☐";
						   }
						   else if (GridType == GridElement.GridAnswerType.Text)
						   {
							   row.Checked = true;
							   if (!string.IsNullOrEmpty(row.Caption))
							   {
								   row.Caption = row.Caption;
							   }
							   else
							   {
								   row.Caption = "risposta";
							   }
						   }
						   UpdateRow(row);
					   }*/
					if (row != null)
						if (row.CellView is EditText)
						{
							row.Caption = ((EditText)row.CellView).Text;
							row.Checked = !string.IsNullOrWhiteSpace(((EditText)row.CellView).Text);
						}
						else
						{
							row.Checked = ((CheckBox)row.CellView).Checked;
							row.Caption = row.Checked ? "☑" : "☐";
						}
				}
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			MenuInflater inflater = MenuInflater;
			//var LeadCapture = Reply.CNHI.LeadCapture.Mobile;
			inflater.Inflate(Resource.Layout.Menu, menu);
			return true;
		}

		public override bool OnPrepareOptionsMenu(IMenu menu)
		{
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.action_done:
					CheckGridForSave();
					OnSave(Source);

					Finish();
					break;

				case Android.Resource.Id.Home: //Tasto Back con Freccia laterale a sinistra
					Finish();
					break;
			}
			return base.OnOptionsItemSelected(item);
		}

		public event EventHandler<GridElement.UserSourceEventArgs> Save;

		private void OnSave(GridElement.UserSource source)
		{
			if (Save != null)
			{
				Save(this, new GridElement.UserSourceEventArgs(source));
			}
		}

		protected override void OnDestroy()
		{
			_instance = null;
			base.OnDestroy();
		}

		protected LayerDrawable getBorders(Color bgColor, Color borderColor,
								  int left, int top, int right, int bottom)
		{
			// Initialize new color drawables
			ColorDrawable borderColorDrawable = new ColorDrawable(borderColor);
			ColorDrawable backgroundColorDrawable = new ColorDrawable(bgColor);

			// Initialize a new array of drawable objects
			Drawable[] drawables = new Drawable[]{
				borderColorDrawable,
				backgroundColorDrawable
		};

			// Initialize a new layer drawable instance from drawables array
			LayerDrawable layerDrawable = new LayerDrawable(drawables);

			// Set padding for background color layer
			layerDrawable.SetLayerInset(
					1, // Index of the drawable to adjust [background color layer]
					left, // Number of pixels to add to the left bound [left border]
					top, // Number of pixels to add to the top bound [top border]
					right, // Number of pixels to add to the right bound [right border]
					bottom // Number of pixels to add to the bottom bound [bottom border]
			);

			// Finally, return the one or more sided bordered background drawable
			return layerDrawable;
		}
	}
}
