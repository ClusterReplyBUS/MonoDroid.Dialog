
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.IO;
using static Android.Support.V4.Widget.DrawerLayout;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;

namespace MonoDroid.Dialog
{
    [Activity(Label = "PhotoActivity")]
    public class CapturePhotoActivity : Activity
    {
        private static volatile CapturePhotoActivity _instance;

        public CapturePhotoActivity() : base()
        {
            if (_instance != null)
            {
                this.Save = _instance.Save;
                this.TitleActivity = _instance.TitleActivity;
                this.ShowSelector = _instance.ShowSelector;
                this.TakePhotoLabel = _instance.TakePhotoLabel;
                this.PickImageLabel = _instance.PickImageLabel;
                this.DeleteButtonLabel = _instance.DeleteButtonLabel;
            }
            _instance = this;
        }

        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            Window.SetTitle(TitleActivity);
        }

        public static CapturePhotoActivity Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CapturePhotoActivity();
                return _instance;
            }
        }

        private enum RequestType
        {
            TakePhoto,
            PickImage,
        }

        protected File _file;
        protected File _dir;
        protected Bitmap _image;

        public bool ShowSelector { get; set; }
        public string TitleActivity { get; set; }
        public string TakePhotoLabel { get; set; }
        public string PickImageLabel { get; set; }
        public string DeleteButtonLabel { get; set; }
        ImageView imageView = null;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActionBar actionBar = ActionBar;
            actionBar.SetBackgroundDrawable(new ColorDrawable(Color.ParseColor("#C3231E"))); // set your desired color
            Display display = WindowManager.DefaultDisplay;
            Point size = new Point();
            display.GetSize(size);
            int width = size.X;
            int height = size.Y;
            imageView = new ImageView(this);


            var layout = new LinearLayout(this);
            layout.SetMinimumWidth(width);
            layout.SetMinimumHeight(height);
            layout.Orientation = Android.Widget.Orientation.Vertical;
            layout.LayoutParameters = (new LayoutParams(LayoutParams.FillParent, LayoutParams.FillParent));

            //SetContentView(Resource.Layout.Photo);
            if (_image != null)
            {
                //var imageView = FindViewById<ImageView>(BaseContext.Resources.GetIdentifier("selectedImage", "id", BaseContext.PackageName));
                //int Iheight = Resources.DisplayMetrics.HeightPixels;
                //int Iwidth = imageView.Height;
                // imageView.LayoutParameters=(new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));
                imageView.SetImageBitmap(_image);
            }
            //Button pickImageBtn = FindViewById<Button>(BaseContext.Resources.GetIdentifier("pickImage", "id", BaseContext.PackageName));
            var pickImageBtn = new Button(this);
            pickImageBtn.Text = PickImageLabel;

            pickImageBtn.Click += delegate
            {
                var imageIntent = new Intent();
                imageIntent.SetType("image/*");
                imageIntent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), (int)RequestType.PickImage);
            };
            pickImageBtn.SetWidth(width);
            pickImageBtn.SetHeight((int)(height / 8));
            //pickImageBtn.LayoutParameters=(new LayoutParams(LayoutParams.MatchParent,100));
            if (!ShowSelector)
                pickImageBtn.Visibility = ViewStates.Invisible;
            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();

                var takePhotoBtn = new Button(this);
                //Button takePhotoBtn = FindViewById<Button>(BaseContext.Resources.GetIdentifier("takePhoto", "id", BaseContext.PackageName));
                takePhotoBtn.Text = TakePhotoLabel;

                takePhotoBtn.Click += (sender, e) =>
               {
                   if (!CameraPermission() || !WriteExternalStoragePermission())
                   {
                       var builder = new AlertDialog.Builder(this);
                       builder.SetTitle("Error");
                       builder.SetMessage("You need to accept required permissions to use this functionality");
                       builder.SetCancelable(false);
                       builder.SetPositiveButton("OK", (senderAlert, args) =>
                      {
                          Finish();
                      });
                       builder.Show();
                   }
                   else
                   {
                       Intent intent = new Intent(MediaStore.ActionImageCapture);

                       _file = new File(_dir, String.Format("dialogPhoto_{0}.jpg", Guid.NewGuid()));

                       intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(_file));

                       StartActivityForResult(intent, (int)RequestType.TakePhoto);
                   }
               };
                takePhotoBtn.SetWidth(width);
                takePhotoBtn.SetHeight((int)(height / 8));
                //takePhotoBtn.LayoutParameters = (new LayoutParams(LayoutParams.MatchParent, 100));

                layout.AddView(pickImageBtn);
                layout.AddView(takePhotoBtn);
                //AddContentView(pickImageBtn,new ViewGroup.LayoutParams(width,100));
                //AddContentView(takePhotoBtn, new ViewGroup.LayoutParams(width, 110));
                if (imageView != null)
                {
                    imageView.SetMinimumWidth(width);
                    imageView.SetMinimumHeight((int)(height * 6 / 8));
                    layout.AddView(imageView);
                    //AddContentView(imageView, new ViewGroup.LayoutParams(width, 300));
                }
                AddContentView(layout, new ViewGroup.LayoutParams(width, height));

                string[] permissions = new string[] { Manifest.Permission.Camera, Manifest.Permission.WriteExternalStorage };
                foreach (var permission in permissions)
                {
                    if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, permission) != (int)Permission.Granted)
                    {
                        ActivityCompat.RequestPermissions(this, permissions, 1001);
                    }
                }
                //if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != (int)Permission.Granted)
                //{
                //    // Permission has never been accepted
                //    // So, I ask the user for permission
                //    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera }, 1001);
                //}

                //if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
                //{
                //    // Permission has never been accepted
                //    // So, I ask the user for permission
                //    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage }, 1);
                //}


            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void CreateDirectoryForPictures()
        {
            _dir = new File(
                Environment.GetExternalStoragePublicDirectory(
                    Environment.DirectoryPictures), "MonoDroid.Dialog");
            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            //var imageView = FindViewById<ImageView>(BaseContext.Resources.GetIdentifier("selectedImage", "id", BaseContext.PackageName));
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = imageView.Height;

            // Make it available in the gallery
            if (requestCode == (int)RequestType.TakePhoto)
            {
                Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                Uri picturePath = Uri.FromFile(_file);
                mediaScanIntent.SetData(picturePath);
                SendBroadcast(mediaScanIntent);

                // Display in ImageView. We will resize the bitmap to fit the display
                // Loading the full sized image will consume to much memory 
                // and cause the application to crash.

                _image = _file.Path.LoadAndResizeBitmap(width, height);
                if (_image != null)
                {
                    imageView.SetImageBitmap(_image);
                }
            }
            else if (requestCode == (int)RequestType.PickImage)
            {
                if (resultCode == Result.Ok)
                {
                    imageView.SetImageURI(data.Data);
                    _image = ((BitmapDrawable)imageView.Drawable).Bitmap;
                }
            }
            // Dispose of the Java side bitmap.
            GC.Collect();
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            MenuInflater inflater = MenuInflater;
            inflater.Inflate(Resource.Layout.Menu, menu);
            var cle = menu.Add(Menu.None, 200, 0, DeleteButtonLabel);
            cle.SetShowAsActionFlags(ShowAsAction.WithText | ShowAsAction.Always);


            //System.Console.WriteLine("CLEAR ID:   " + cle.ItemId);
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
                    OnSave(_image);
                    Finish();
                    break;

                case Android.Resource.Id.Home: //Tasto Back con Freccia laterale a sinistra
                    Finish();
                    break;

                case 200:
                    _image = null;
                    OnSave(_image);
                    Finish();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
        public event EventHandler<BitmapEventArgs> Save;

        private void OnSave(Bitmap source)
        {
            //if (Save != null)
            //{
            Save(this, new BitmapEventArgs(source));
            //}
        }
        protected override void OnDestroy()
        {
            _instance = null;
            base.OnDestroy();
        }


        private bool CameraPermission()
        {
            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) == (int)Permission.Granted)
                return true;
            return false;
        }

        private bool WriteExternalStoragePermission()
        {
            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted)
                return true;
            return false;
        }

    }


    public class BitmapEventArgs : EventArgs
    {
        public Bitmap Value { get; private set; }
        public BitmapEventArgs(Bitmap arg)
        {
            Value = arg;
        }
    }

}