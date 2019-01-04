using System;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
    public abstract class BoolElement : Element
    {
        private bool _val;

        public virtual bool Value
        {
            get { return _val; }
            set
            {
                if (_val != value)
                {
                    _val = value;
                    if (ValueChanged != null)
                        ValueChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler ValueChanged;

        public BoolElement(string caption, bool value) : base(caption)
        {
            _val = value;
        }

        public BoolElement(string caption, bool value, int layoutId)
            : base(caption, layoutId)
        {
            _val = value;
        }
        
        public override string Summary()
        {
            return _val ? "On" : "Off";
        }
    }

    /// <summary>
    /// Used to display toggle button on the screen.
    /// </summary>
    public class BooleanElement : BoolElement, CompoundButton.IOnCheckedChangeListener
    {
		protected ToggleButton _toggleButton;
		protected TextView _caption;
        private TextView _subCaption;

        public BooleanElement(string caption, bool value)
            : base(caption, value, (int) DroidResources.ElementLayout.dialog_onofffieldright)
        {
        }

        public BooleanElement(string caption, bool value, int layoutId)
            : base(caption, value, layoutId)
        {
        }

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            View toggleButtonView;
            View view = DroidResources.LoadBooleanElementLayout(context, convertView, parent, LayoutId, out _caption, out _subCaption, out toggleButtonView);

            if (view != null)
            {
                _caption.Text = Caption;

                if (this.IsMissing)
                    _caption.SetTextColor(Color.ParseColor(Colors.MissingRed));

                _toggleButton = toggleButtonView as ToggleButton;
                _toggleButton.SetOnCheckedChangeListener(null);
                _toggleButton.Checked = Value;
                _toggleButton.SetOnCheckedChangeListener(this);
            }
            return view;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_toggleButton.Dispose();
                _toggleButton = null;
                //_caption.Dispose();
                _caption = null;
            }
        }

		//public override ListView GetCell(ListView tv)
		//{
		//	return tv;
			//if (_toggleButton == null)
			//{
			//	_toggleButton = new ToggleButton(GetContext());

			//	_toggleButton.SetBackgroundColor(Color.Transparent);
			//	Tag = 1;
			//	//On = Value
			//	_toggleButton.CheckedChange += delegate
			//	{

			//	};
			//	//_toggleButton.AddTarget(delegate
			//	//{
			//	//Value = sw.On;
			//	//}, UIControlEvent.ValueChanged);
			//}
			//else {
			//	//sw.On = Value;
			//}

			//var cell = tv.DequeueReusableCell(CellKey);
			//if (cell == null)
			//{
			//	cell = new UITableViewCell(UITableViewCellStyle.Default, CellKey);
			//	cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			//}
			//else
			//	RemoveTag(cell, 1);

			//cell.TextLabel.Text = Caption;
			//cell.AccessoryView = sw;

			//return cell;
		
		//public override UITableViewCell GetCell(UITableView tv)
		//{
		//	if (sw == null)
		//	{
		//		sw = new UISwitch()
		//		{
		//			BackgroundColor = UIColor.Clear,
		//			Tag = 1,
		//			On = Value
		//		};
		//		sw.AddTarget(delegate
		//		{
		//			Value = sw.On;
		//		}, UIControlEvent.ValueChanged);
		//	}
		//	else
		//		sw.On = Value;

		//	var cell = tv.DequeueReusableCell(CellKey);
		//	if (cell == null)
		//	{
		//		cell = new UITableViewCell(UITableViewCellStyle.Default, CellKey);
		//		cell.SelectionStyle = UITableViewCellSelectionStyle.None;
		//	}
		//	else
		//		RemoveTag(cell, 1);

		//	cell.TextLabel.Text = Caption;
		//	cell.AccessoryView = sw;

		//	return cell;
		//}

		//}



        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            this.Value = isChecked;
			this._caption.SetBackgroundColor(Color.ParseColor("#FAFAD2"));
	
			//_subCaption.SetBackgroundColor(Color.ParseColor("#FAFAD2"));
        }
    }
}