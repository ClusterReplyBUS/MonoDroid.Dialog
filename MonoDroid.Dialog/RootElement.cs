using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace MonoDroid.Dialog
{
    public class
    RootElement : Element, IEnumerable<Section>, IDialogInterfaceOnClickListener
    {
        TextView _caption;
        protected TextView _value;
        public ListView TableView;
        int _summarySection, _summaryElement;
        internal Group _group;
        public bool UnevenRows;
        public Func<RootElement, View> _createOnSelected;
        protected string _showValue;

        private Dictionary<Guid,CheckboxElement> _multipleItems;

        /// <summary>
        ///  Initializes a RootSection with a caption
        /// </summary>
        /// <param name="caption">
        ///  The caption to render.
        /// </param>
        public RootElement(string caption)
            : base(caption, (int)DroidResources.ElementLayout.dialog_root)
        {
            _summarySection = -1;
            Sections = new List<Section>();
        }

        /// <summary>
        /// Initializes a RootSection with a caption and a callback that will
        /// create the nested UIViewController that is activated when the user
        /// taps on the element.
        /// </summary>
        /// <param name="caption">
        ///  The caption to render.
        /// </param>
        public RootElement(string caption, Func<RootElement, View> createOnSelected)
            : base(caption, (int)DroidResources.ElementLayout.dialog_root)
        {
            _summarySection = -1;
            this._createOnSelected = createOnSelected;
            Sections = new List<Section>();
        }

        /// <summary>
        ///   Initializes a RootElement with a caption with a summary fetched from the specified section and leement
        /// </summary>
        /// <param name="caption">
        /// The caption to render cref="System.String"/>
        /// </param>
        /// <param name="section">
        /// The section that contains the element with the summary.
        /// </param>
        /// <param name="element">
        /// The element index inside the section that contains the summary for this RootSection.
        /// </param>
        public RootElement(string caption, int section, int element)
            : base(caption, (int)DroidResources.ElementLayout.dialog_root)
        {
            _summarySection = section;
            _summaryElement = element;
        }

        /// <summary>
        /// Initializes a RootElement that renders the summary based on the radio settings of the contained elements. 
        /// </summary>
        /// <param name="caption">
        /// The caption to ender
        /// </param>
        /// <param name="group">
        /// The group that contains the checkbox or radio information.  This is used to display
        /// the summary information when a RootElement is rendered inside a section.
        /// </param>
        public RootElement(string caption, Group group)
            : base(caption, (int)DroidResources.ElementLayout.dialog_root)
        {
            this._group = group;
        }

        /// <summary>
        /// Single save point for a context, elements can get this context via GetContext() for navigation operations
        /// </summary>
        public Context Context { get; set; }

        internal List<Section> Sections = new List<Section>();

        public int Count
        {
            get
            {
                return Sections.Count;
            }
        }

        public Section this[int idx]
        {
            get
            {
                return Sections[idx];
            }
        }

        internal int IndexOf(Section target)
        {
            int idx = 0;
            foreach (Section s in Sections)
            {
                if (s == target)
                    return idx;
                idx++;
            }
            return -1;
        }

        internal void Prepare()
        {
            int current = 0;
            foreach (Section s in Sections)
            {
                foreach (Element e in s.Elements)
                {
                    var re = e as RadioElement;
                    if (re != null)
                        re.RadioIdx = current++;
                    if (UnevenRows == false && e is IElementSizing)
                        UnevenRows = true;
                }
            }
        }

        public override string Summary()
        {
            return GetSelectedValue();
        }

        void SetSectionStartIndex()
        {
            int currentIndex = 0;
            foreach (var section in Sections)
            {
                section.StartIndex = currentIndex;
                currentIndex += section.Count;
            }
        }
        /// <summary>
        /// Adds a new section to this RootElement
        /// </summary>
        /// <param name="section">
        /// The section to add, if the root is visible, the section is inserted with no animation
        /// </param>
        public void Add(Section section)
        {
            if (section == null)
                return;

            Sections.Add(section);
            section.Parent = this;
            SetSectionStartIndex();
        }

        //
        // This makes things LINQ friendly;  You can now create RootElements
        // with an embedded LINQ expression, like this:
        // new RootElement ("Title") {
        //     from x in names
        //         select new Section (x) { new StringElement ("Sample") }
        //
        public void Add(IEnumerable<Section> sections)
        {
            foreach (var s in sections)
                Add(s);

            SetSectionStartIndex();
        }

        /// <summary>
        /// Inserts a new section into the RootElement
        /// </summary>
        /// <param name="idx">
        /// The index where the section is added <see cref="System.Int32"/>
        /// </param>
        /// <param name="newSections">
        /// A <see cref="Section[]"/> list of sections to insert
        /// </param>
        /// <remarks>
        ///    This inserts the specified list of sections (a params argument) into the
        ///    root using the specified animation.
        /// </remarks>
        public void Insert(int idx, params Section[] newSections)
        {
            if (idx < 0 || idx > Sections.Count)
                return;
            if (newSections == null)
                return;

            //if (Table != null)
            //    Table.BeginUpdates();

            int pos = idx;
            foreach (var s in newSections)
            {
                s.Parent = this;
                Sections.Insert(pos++, s);
            }

            SetSectionStartIndex();
        }

        /// <summary>
        /// Removes a section at a specified location
        /// </summary>
        public void RemoveAt(int idx)
        {
            if (idx < 0 || idx >= Sections.Count)
                return;

            Sections.RemoveAt(idx);

            SetSectionStartIndex();
        }

        public void Remove(Section s)
        {
            if (s == null)
                return;
            int idx = Sections.IndexOf(s);
            if (idx == -1)
                return;
            RemoveAt(idx);

            SetSectionStartIndex();
        }

        public void Clear()
        {
            foreach (var s in Sections)
                s.Dispose();
            Sections = new List<Section>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context = null;
                if (Sections == null)
                    return;
                Clear();
                Sections = null;
            }
        }

        /// <summary>
        /// The currently selected Radio item in the whole Root.
        /// </summary>
        public int RadioSelected
        {
            get
            {
                var radio = _group as RadioGroup;
                if (radio != null)
                    return radio.Selected;
                return -1;
            }
            set
            {
                var radio = _group as RadioGroup;
                if (radio != null)
                    radio.Selected = value;
            }
        }

        private string GetSelectedValue()
        {
            var radio = _group as RadioGroup;
            if (radio == null)
                return string.Empty;

            int selected = radio.Selected;
            int current = 0;
            string radioValue = string.Empty;
            foreach (var s in Sections)
            {
                foreach (var e in s.Elements)
                {
                    if (!(e is RadioElement))
                        continue;

                    if (current == selected)
                        return e.Summary();

                    current++;
                }
            }

            return string.Empty;

        }

        bool _clickAssigned = false;

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            Context = context;

            //LayoutInflater inflater = LayoutInflater.FromContext(context);
            View cell = new TextView(context) { TextSize = 16f, Text = Caption };
            var radio = _group as RadioGroup;


            if (radio != null)
            {
                string radioValue = GetSelectedValue();

                //Come passo booleano readonly dell'elemento???
                //cell = DroidResources.LoadReadOnlyStringElementLayout(context, convertView, parent, LayoutId, out _caption, out _value);
                cell = DroidResources.LoadStringElementLayout(context, convertView, parent, LayoutId, out _caption, out _value);
                if (cell != null)
                {
                    _caption.Text = Caption;

                    if (this.IsMissing)
                        _caption.SetTextColor(Color.ParseColor(Colors.MissingRed));

<<<<<<< Updated upstream
                    _value.Text = radioValue;
					if (!string.IsNullOrEmpty(radioValue))
					{ 
						_value.SetBackgroundColor(Color.ParseColor("#FAFAD2"));
					}
					//this.LongClick += SelectRadio;
					if (this.Click == null)
						this.Click += SelectRadio;
				}
			}
			else if (_group != null)
			{
				int count = 0;
				foreach (var s in Sections)
				{
					foreach (var e in s.Elements)
					{
						var ce = e as CheckboxElement;
						if (ce != null)
						{
							if (ce.Value)
								count++;
							continue;
						}
						var be = e as BoolElement;
						if (be != null)
						{
							if (be.Value)
								count++;
							continue;
						}
					}

				}
				cell = DroidResources.LoadStringElementLayout(context, convertView, parent, LayoutId, out _caption, out _value);
				if (cell != null)
				{
					_caption.Text = Caption;
                    if (this.IsMissing)
                        _caption.SetTextColor(Color.ParseColor(Colors.MissingRed));

                    if (_showValue != "0")
					{
						_value.Text = _showValue;
					}
					else
					{
						_value.Text = "0";
					}
					int cont = int.Parse(_showValue);
					if (cont >= 1)
					{ 
						_value.SetBackgroundColor(Color.ParseColor("#FAFAD2"));
					}
					if (this.Click == null)
						this.Click += SelectCheckBox;
					//this.LongClick += SelectCheckBox;
				}
			}
			else if (_summarySection != -1 && _summarySection < Sections.Count)
			{
				var s = Sections[_summarySection];
				//if (summaryElement < s.Elements.Count)
				//    cell.DetailTextLabel.Text = s.Elements[summaryElement].Summary();
			}
			//cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

			return cell;
		}

		public void SelectCheckBox()
		{
			var items = new Dictionary<string, bool>();
			foreach (var s in Sections)
			{
				foreach (var e in s.Elements)
				{
					if (e is CheckboxElement)
						items.Add(e.Caption, ((CheckboxElement)e).Value);
				}
			}
			var dialog = new AlertDialog.Builder(Context);
			dialog.SetMultiChoiceItems(items.Keys.ToArray(), items.Values.ToArray(), (sender, e) =>
			{
				OnChildSelected(e.Which);
			});
			dialog.SetTitle(this.Caption);
			dialog.SetNegativeButton("Cancel", this);
			dialog.SetPositiveButton("OK", (object sender, DialogClickEventArgs e) =>
			 { 
			_value.Text = _showValue;
			});

			dialog.Create().Show();
		}

		public void SelectRadio()
		{
			List<string> items = new List<string>();
			foreach (var s in Sections)
			{
				foreach (var e in s.Elements)
				{
					if (e is RadioElement)
						if (e.Summary() != null)
							items.Add(e.Summary());
						else
							items.Add(string.Empty);
				}
			}
			var dialog = new AlertDialog.Builder(Context);
			dialog.SetSingleChoiceItems(items.ToArray(), this.RadioSelected, this);
			dialog.SetTitle(this.Caption);
			dialog.SetNegativeButton("Cancel", this);
			dialog.Create().Show();
		}
		void IDialogInterfaceOnClickListener.OnClick(IDialogInterface dialog, int which)
		{
			if ((int)which >= 0)
			{
				this.RadioSelected = (int)which;
				OnChildSelected(which);
				string radioValue = GetSelectedValue();
				_value.Text = radioValue;
			}
			dialog.Dismiss();
		}

		protected virtual void OnChildSelected(int which)
		{
		}
=======
                    _value.Text = radioValue;
                    if (!string.IsNullOrEmpty(radioValue))
                    {
                        _value.SetBackgroundColor(Color.ParseColor("#FAFAD2"));
                    }
                    //this.LongClick += SelectRadio;
                    if (this.Click == null)
                        this.Click += SelectRadio;
                }
            }
            else if (_group != null)
            {
                int count = 0;
                foreach (var s in Sections)
                {
                    foreach (var e in s.Elements)
                    {
                        var ce = e as CheckboxElement;
                        if (ce != null)
                        {
                            if (ce.Value)
                                count++;
                            continue;
                        }
                        var be = e as BoolElement;
                        if (be != null)
                        {
                            if (be.Value)
                                count++;
                            continue;
                        }
                    }

                }
                cell = DroidResources.LoadStringElementLayout(context, convertView, parent, LayoutId, out _caption, out _value);
                if (cell != null)
                {
                    _caption.Text = Caption;

                    if (_showValue != "0")
                    {
                        _value.Text = _showValue;
                    }
                    else
                    {
                        _value.Text = "0";
                    }
                    int cont = int.Parse(_showValue);
                    if (cont >= 1)
                    {
                        _value.SetBackgroundColor(Color.ParseColor("#FAFAD2"));
                    }
                    if (this.Click == null)
                        this.Click += SelectCheckBox;
                    //this.LongClick += SelectCheckBox;
                }
            }
            else if (_summarySection != -1 && _summarySection < Sections.Count)
            {
                var s = Sections[_summarySection];
                //if (summaryElement < s.Elements.Count)
                //    cell.DetailTextLabel.Text = s.Elements[summaryElement].Summary();
            }
            //cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

            return cell;
        }

        public void SelectCheckBox()
        {
            /*var items = new Dictionary<string, bool>();
            foreach (var s in Sections)
                {
                foreach (var e in s.Elements)
                {
                    if (e is CheckboxElement)
                        items.Add(e.Caption, ((CheckboxElement)e).Value);
                }
            }

            this._multipleItems = new Dictionary<string, bool>(items);
            AlertDialog alert = null;
            var dialog = new AlertDialog.Builder(Context);

            SearchView searchView = new SearchView(Context);
            dialog.SetView(searchView);

            dialog.SetMultiChoiceItems(items.Keys.ToArray(), items.Values.ToArray(), (sender, e) => OnItemSelected(sender, e));

            dialog.SetTitle(this.Caption);
            dialog.SetNegativeButton("Cancel", this);
            dialog.SetPositiveButton("OK", (object sender, DialogClickEventArgs e) =>
             {
                 _value.Text = _showValue;
             });

            searchView.QueryTextChange += (sender, e) => Search(sender, e.NewText, alert.ListView, items, null,dialog);

            alert = dialog.Create();
            alert.Show();*/

            /*var builder = new AlertDialog.Builder(Context);
            builder.SetNegativeButton("Cancel", this);
            builder.SetPositiveButton("OK", (object sender, DialogClickEventArgs e) =>
            {
                _value.Text = _showValue;
            });

            var dlgAlert = builder.Create();
            dlgAlert.SetTitle(this.Caption);

            var li = LayoutInflater.FromContext(Context);
            var viewSL = li.Inflate(Resource.Layout.searchablelist, null);

            var listView = viewSL.FindViewById<ListView>(Resource.Id.List);
            listView.ChoiceMode = ChoiceMode.Multiple;
            listView.Adapter = new CheckBoxAdapter(li, items, (index, ischecked) => OnItemSelected(listView, index, ischecked)  );
            listView.ItemSelected += ListView_ItemSelected;
            
            //listView.ItemClick += (sender, e) => OnItemSelected((ListView)sender, e, items, dlgAlert);

            var editBox = viewSL.FindViewById<EditText>(Resource.Id.EditBox);
            editBox.TextChanged += (sender, e) => Search(sender, e.Text.ToString(), listView, items, null, builder);

            dlgAlert.SetView(viewSL);
            //dlgAlert.SetButton("Cancel", handlerCancelButton);
            dlgAlert.Show();*/
            var items = new List<CheckboxElement>();
            foreach (var s in Sections)
            {
                foreach (var e in s.Elements)
                {
                    if (e is CheckboxElement)
                        items.Add(e as CheckboxElement);
                }
            }
            this._multipleItems = items.ToDictionary(i=>(Guid)i.Tag, i=>i);

            var builder = new AlertDialog.Builder(Context);
            builder.SetNegativeButton("Cancel", this);
            builder.SetPositiveButton("OK", (object sender, DialogClickEventArgs e) =>
            {
                _value.Text = _showValue;
            });


            var dlgAlert = builder.Create();
            dlgAlert.SetTitle(this.Caption);
            var viewSL = LayoutInflater.FromContext(Context).Inflate(Resource.Layout.searchablelist, null);

            var listView = viewSL.FindViewById<ListView>(Resource.Id.List);
            listView.ChoiceMode = ChoiceMode.Multiple;
            listView.Adapter = new CheckBoxAdapter(Context, items, (elem) => OnItemSelected(listView, elem));
            //listView.ItemClick += (sender, e) => OnClick((ListView)sender, e, items.Keys.ToList(), dlgAlert);
            for (int i = 0; i < items.Count; i++)
                listView.SetItemChecked(i, items[i].Value);

            var editBox = viewSL.FindViewById<SearchView>(Resource.Id.EditBox);
            editBox.QueryTextChange += (sender, e) => Search(e.NewText ?? string.Empty, listView, _multipleItems.ToDictionary(i=>i.Value.Caption, i=>i.Value.Value), null);

            dlgAlert.SetView(viewSL);
            dlgAlert.Show();
        }

        private void OnItemSelected(ListView listView, CheckboxElement element)
        {
            //int realPosition = GetRealItemPosition(listView, index);
            //_multipleItems[realPosition].Value = isChecked;
            OnChildSelected(_multipleItems.Keys.ToList().IndexOf((Guid)element.Tag));

            // OnChildSelected(e.Which);
        }
        /*
        private void Search(object sender, string newText, ListView listView, Dictionary<string, bool> multipleItems = null, List<string> singleItems = null, AlertDialog.Builder dialog = null)
        {
            var selected = listView.CheckedItemPositions;

            bool isSingleChoice = singleItems != null ? true : false;

            if (newText.Length >= 2)
            {
                listView.TextFilterEnabled = false;
                if (isSingleChoice)
                {

                    var sorted = singleItems
                                       .Where(x => x.ToLower().Contains(newText.ToLower()))
                                       .OrderBy(x=>x).ToList();
                    listView.SetItemChecked(sorted.IndexOf(singleItems[this.RadioSelected]), true);
                    listView.RequestLayout();
                    ((BaseAdapter)listView.Adapter) .NotifyDataSetChanged();
                }
                else
                {

                    var sorted = multipleItems
                                       .Where(x => x.Key.ToLower().Contains(newText.ToLower()))
                                       .Concat(multipleItems
                                              .Where(x => !x.Key.ToLower().Contains(newText.ToLower()))
                                              ).ToDictionary(k => k.Key, k => k.Value);


                    SetMultipleChoiceItems(dialog, listView, sorted);
                }

                listView.TextFilterEnabled = true;
                listView.SetFilterText(newText);
            }

            else if(listView.HasTextFilter)
            {
                
                //if (isSingleChoice)
                //{
                //    listView.SetItemChecked(this.RadioSelected, true);
                //}
                //else
                //{
                //    SetMultipleChoiceItems(dialog, listView, multipleItems);
                //}
                listView.ClearTextFilter();

                listView.TextFilterEnabled = false;

            }

        }
        */
        private void Search(string newText, ListView listView, Dictionary<string, bool> multipleItems = null, List<string> singleItems = null)
        {
            var selected = listView.CheckedItemPositions;

            bool isSingleChoice = singleItems != null ? true : false;

            if (newText.Length >= 2)
            {
                listView.TextFilterEnabled = false;
                if (isSingleChoice)
                {

                    //var sorted = singleItems
                    //                   .Where(x => x.ToLower().Contains(newText.ToLower()))
                    //                   .OrderBy(x => x).ToList();
                    //listView.SetItemChecked(sorted.IndexOf(singleItems[this.RadioSelected]), true);
                    listView.RequestLayout();
                    ((BaseAdapter)listView.Adapter).NotifyDataSetChanged();
                }
                else
                {

                    //var sorted = multipleItems
                    //                   .Where(x => x.Key.ToLower().Contains(newText.ToLower()))
                    //                   .Concat(multipleItems
                    //                          .Where(x => !x.Key.ToLower().Contains(newText.ToLower()))
                    //                          ).ToDictionary(k => k.Key, k => k.Value);
                    //var sorted = multipleItems
                    //                   .Where(x => x.Key.ToLower().Contains(newText.ToLower()))
                    //                   .OrderBy(x => x).ToDictionary(k => k.Key, k => k.Value);

                    //foreach(var item in sorted)
                    //    listView.SetItemChecked(multipleItems.Keys.ToList().IndexOf(item.Key), true);

                    //listView.CheckedItemPositions.Clear();
                    //listView.ClearChoices();
                    listView.RequestLayout();
                    ((BaseAdapter)listView.Adapter).NotifyDataSetChanged();
                }

                listView.TextFilterEnabled = true;
                listView.SetFilterText(newText);
            }

            else if (listView.HasTextFilter)
            {

                //if (isSingleChoice)
                //{
                //    listView.SetItemChecked(this.RadioSelected, true);
                //}
                //else
                //{
                //    SetMultipleChoiceItems(dialog, listView, multipleItems);
                //}
                listView.ClearTextFilter();

                listView.TextFilterEnabled = false;

            }

        }


        private void SetMultipleChoiceItems(AlertDialog.Builder dialog, ListView listView, Dictionary<string, bool> items)
        {
            //dialog.SetMultiChoiceItems(items.Keys.ToArray(), items.Values.ToArray(), (sender, e) => OnItemSelected(sender, e));
            //listView.CheckedItemPositions.Clear();
            //listView.ClearChoices();
            //listView.RequestLayout();
            ((BaseAdapter)listView.Adapter).NotifyDataSetChanged();
        }

      

        //private int GetRealItemPosition(ListView listView, int which)
        //{
        //    var checkedItem = (CheckboxElement)listView.GetItemAtPosition(which);
        //    return _multipleItems.IndexOf(checkedItem);
        //}

        private int GetRealItemPosition(ListView listView, List<string> originalItems, int which)
        {
            var checkedItem = listView.GetItemAtPosition(which);
            return originalItems.IndexOf(checkedItem.ToString());
        }

        public void SelectRadio()
        {
            List<string> items = new List<string>();

            foreach (var s in Sections)
            {
                foreach (var e in s.Elements)
                {
                    if (e is RadioElement)
                        if (e.Summary() != null)
                            items.Add(e.Summary());
                        else
                            items.Add(string.Empty);
                }
            }
            /*SearchView searchView = new SearchView(Context);
            
            var dialog = new AlertDialog.Builder(Context);
            dialog.SetView(searchView);
            dialog.SetSingleChoiceItems(items.ToArray(), this.RadioSelected, (dialog, e) => { OnClick(dialog, e, items); });  //this);
            dialog.SetTitle(this.Caption);
            dialog.SetNegativeButton("Cancel", this);

            var alert = dialog.Create();
            searchView.QueryTextChange += (sender, e) => Search(sender, e, alert, null, items,dialog);
            alert.Show();*/

            var builder  = new AlertDialog.Builder(Context);
            builder.SetNegativeButton("Cancel", this);

            var dlgAlert = builder.Create();
            dlgAlert.SetTitle(this.Caption);
            var viewSL = LayoutInflater.FromContext(Context).Inflate(Resource.Layout.searchablelist, null);

            var listView = viewSL.FindViewById<ListView>(Resource.Id.List);
            listView.Adapter = new ArrayAdapter<string>(Context, Resource.Layout.simple_list_item_1, items);
            listView.ItemClick += (sender, e) => OnClick((ListView)sender, e, items, dlgAlert);
            
            var editBox = viewSL.FindViewById<SearchView>(Resource.Id.EditBox);
            editBox.QueryTextChange += (sender, e) => Search(e.NewText ?? string.Empty, listView, null, items);

            dlgAlert.SetView(viewSL);
            //dlgAlert.SetButton("Cancel", handlerCancelButton);
            dlgAlert.Show();
        }
        void handlerCancelButton(object sender, DialogClickEventArgs e)
        {
            AlertDialog objAlertDialog = sender as AlertDialog;
            Button btnClicked = objAlertDialog.GetButton(e.Which);
            Toast.MakeText(Context, "you clicked on " + btnClicked.Text, ToastLength.Long).Show();
        }

        void OnClick(ListView sender, AdapterView.ItemClickEventArgs e, List<string> originalItems, AlertDialog dialog)
        {
            this.RadioSelected = GetRealItemPosition(sender, originalItems, e.Position);  // (int)which;
            OnChildSelected(RadioSelected);
            string radioValue = GetSelectedValue();
            _value.Text = radioValue;

            dialog.Dismiss();
        }


        /*void IDialogInterfaceOnClickListener.OnClick(IDialogInterface dialog, int which,List<string> originalItems)
        {
            if ((int)which >= 0)
            {
                this.RadioSelected = GetRealItemPosition(((AlertDialog)dialog).ListView, originalItems, which);  // (int)which;
                OnChildSelected(which);
                string radioValue = GetSelectedValue();
                _value.Text = radioValue;
            }
            dialog.Dismiss();
        } */

        protected virtual void OnChildSelected(int which)
        {
        }

        /// <summary>
        /// Enumerator that returns all the sections in the RootElement.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator"/>
        /// </returns>
        public IEnumerator<Section> GetEnumerator()
        {
            return Sections.GetEnumerator();
        }

        /// <summary>
        /// Enumerator that returns all the sections in the RootElement.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Sections.GetEnumerator();
        }

        public void OnClick(IDialogInterface dialog, int which)
        {
            dialog.Dismiss();
        }
    }
>>>>>>> Stashed changes


}