using System;
using Android.Content;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.App;
using Android.Views.InputMethods;
using System.Collections.Generic;
using Android.Graphics;

namespace MonoDroid.Dialog
{
	public class EntryElement : Element, ITextWatcher
	{

		public string Value
		{
			get { return _val; }
			set
			{
				_val = value;
				if (_entry != null && _entry.Text != value)
				{
					_entry.Text = value;
					if (ValueChanged != null)
						ValueChanged(this, EventArgs.Empty);
				}
			}
		}

		public event EventHandler ValueChanged;

		public EntryElement(string caption, string value)
			: this(caption, value, (int)DroidResources.ElementLayout.dialog_textfieldright)
		{

		}

		public EntryElement(string caption, string hint, string value) : this(caption, value)
		{
			Hint = hint;
		}

		public EntryElement(string caption, string @value, int layoutId)
			: base(caption, layoutId)
		{
			_val = @value;
			Lines = 1;
		}

		public override string Summary()
		{
			return _val;
		}

		public bool Password { get; set; }
		public bool Numeric { get; set; }
		public string Hint { get; set; }
		public int Lines { get; set; }

		protected EditText _entry;
		protected TextView _label;
		protected string _val;
		protected Action _entryClicked { get; set; }



		public override View GetView(Context context, View convertView, ViewGroup parent)
		{
			Log.Debug("MDD", "EntryElement: GetView: ConvertView: " + ((convertView == null) ? "false" : "true") +
				" Value: " + Value + " Hint: " + Hint + " Password: " + (Password ? "true" : "false"));


			var view = DroidResources.LoadStringEntryLayout(context, convertView, parent, LayoutId, out _label, out _entry);
			if (view != null)
			{

				// Warning! Crazy ass hack ahead!
				// since we can't know when out convertedView was was swapped from inside us, we store the
				// old textwatcher in the tag element so it can be removed!!!! (barf, rech, yucky!)
				if (_entry.Tag != null)
					_entry.RemoveTextChangedListener((ITextWatcher)_entry.Tag);

				_entry.Text = this.Value ?? String.Empty;
				_entry.Hint = this.Hint;



				//var p=((ViewGroup)view).GetChildAt(1);
				//var p1 = ((ViewGroup)view).GetChildAt(2);
				//				p.Click += delegate
				//				 {
				//					 HoCliccato();
				//				 };


				if (this.Password)
					_entry.InputType = (InputTypes.ClassText | InputTypes.TextVariationPassword);
				else if (this.Numeric)
					_entry.InputType = (InputTypes.ClassNumber | InputTypes.NumberFlagDecimal | InputTypes.NumberFlagSigned);
				else
					_entry.InputType = InputTypes.ClassText;

				// continuation of crazy ass hack, stash away the listener value so we can look it up later
				_entry.Tag = this;
				_entry.AddTextChangedListener(this);
				_label.Text = (_label != null) ? Caption : string.Empty;

                if (this.IsMissing)
                    _label.SetTextColor(Color.ParseColor(Colors.MissingRed));
                _label.Invalidate();
            }

            /*this.Click += () =>
			{
				if (_entry != null)
				{
					var contex = this.GetContext();
					var entryDialog = new AlertDialog.Builder(contex)
						.SetTitle("Enter Text")
						.SetView(_entry)
						.SetPositiveButton("OK", (o, e) =>
						{
							this.Value = _entry.Text;
						})
						.Create();

					entryDialog.Show();
				}
			};*/

            //if (((int)Android.OS.Build.VERSION.SdkInt)==21 ||( (int)Android.OS.Build.VERSION.SdkInt)== 22)
            //{
            //	for (int i = 0; i < ((ViewGroup)view).ChildCount; i++)
            //	{
            //		var item = ((ViewGroup)view).GetChildAt(i);

            //		item.Click += delegate
            //		{
            //			item.RequestFocus();
            //			changeStatusItem(item);
            //		};

            //	}
            //}
            //else

            if (((int)Android.OS.Build.VERSION.SdkInt) < 21 || ((int)Android.OS.Build.VERSION.SdkInt) > 22)
			{
				_entry.FocusChange += _entry_FocusChange;
			}


			return view;

		}

		//protected virtual void changeStatusItem(View item)
		//{
		//	throw new NotImplementedException();
		//}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				//_entry.Dispose();
				_entry = null;
			}
		}

		public override bool Matches(string text)
		{
			return (Value != null ? Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 : false) || base.Matches(text);
		}

		public void OnTextChanged(Java.Lang.ICharSequence s, int start, int before, int count)
		{
			this.Value = s.ToString();
		}

		public void AfterTextChanged(IEditable s)
		{
			//Console.Write("foo");
			// nothing needed
		}

		public void BeforeTextChanged(Java.Lang.ICharSequence s, int start, int count, int after)
		{
			//Console.Write("foo");
			// nothing needed
		}


		public void OnChildSelected()
		{
			if (OnSelected != null)
				OnSelected(this, null);
		}
		public event EventHandler<EventArgs> OnSelected;

		void _entry_FocusChange(object sender, View.FocusChangeEventArgs e)
		{
            if (((int)Android.OS.Build.VERSION.SdkInt)==19) // Versione 4.4 (KitKat)
            {
                var view = sender as Android.Views.View;
                if (!e.HasFocus)
                {
                    var inputMethodManager = view.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
                    inputMethodManager.HideSoftInputFromWindow(view.WindowToken, 0);
                }   
            }	
		}
	}
}



