using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
    public class ImageElement : Element
    {
		// Height for rows
		const int dimx = 48;
		const int dimy = 44;
		
		// radius for rounding
		const int roundPx = 12;

        public Bitmap Value
        {
            get { return _image; }
            set
            {
                _image = value;
                if (_imageView != null)
                {
                    _imageView.SetImageBitmap(_image);
                }
            }
        }
        Bitmap _image;

        ImageView _imageView;

		public ImageElement(string caption, Bitmap image) : base(caption)
		{
			_image = image;
			
		}

		public ImageElement(Bitmap image) : this("", image)
        {
		}
				
		protected override void Dispose (bool disposing)
		{
            if (disposing)
            {
				_image.Dispose();
			}
			base.Dispose (disposing);
		}

        public override View GetView(Context context, View convertView, ViewGroup parent)
		{
            this.Click = delegate { SelectImage(); };

			Bitmap scaledBitmap = null;
			if (_image != null)
				scaledBitmap = Bitmap.CreateScaledBitmap(_image, dimx, dimy, true);

            var view = convertView as RelativeLayout;
            if (view == null)
            {
                view = new RelativeLayout(context);
                _imageView = new ImageView(context);
            }
            else
            {
                _imageView = (ImageView)view.GetChildAt(0);
            }
			if (scaledBitmap != null) 
            _imageView.SetImageBitmap(scaledBitmap);
            
            var parms = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            parms.SetMargins(5, 2, 5, 2);
            parms.AddRule( LayoutRules.AlignParentLeft);
			if(_imageView.Parent != null && _imageView.Parent is ViewGroup)
				((ViewGroup)_imageView.Parent).RemoveView(_imageView);
			view.AddView(_imageView, parms);

            return view;
		}

		protected virtual void SelectImage()
        {
            Context context = GetContext();
            Activity activity = (Activity)context;
            Intent intent = new Intent(Intent.ActionPick, Android.Provider.MediaStore.Images.Media.InternalContentUri);
            activity.StartActivityForResult(intent,1);
        }
	}
}