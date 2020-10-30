using System;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Media;

public static class BitmapHelpers
{
	public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
	{
		// First we get the the dimensions of the file on disk
		BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = false };
		BitmapFactory.DecodeFile(fileName, options);

		// Next we calculate the ratio that we need to resize the image by
		// in order to fit the requested dimensions.
		int outHeight = options.OutHeight;
		int outWidth = options.OutWidth;

		int inSampleSize = 1;


           if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            } 

        // Now we will load the image and have BitmapFactory resize it for us.
        // options.InSampleSize = calculateInSampleSize(options,width,height);
        options.InSampleSize = inSampleSize;
        Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);
        ExifInterface exif = null;
		try
		{
			exif = new ExifInterface(fileName);
		}
		catch (IOException e)
		{
			System.Console.WriteLine(e.Message + e.StackTrace);
            return null;
		}

		int orientation = 0;
		if (exif != null)
			orientation = exif.GetAttributeInt(ExifInterface.TagOrientation, 0);

		switch (orientation)
		{
			case (int)Orientation.Rotate90:
                resizedBitmap = RotateBitmap(resizedBitmap, 90);
				break;
			case (int)Orientation.Rotate180:
                resizedBitmap = RotateBitmap(resizedBitmap, 180);
				break;

			case (int)Orientation.Rotate270:
                resizedBitmap = RotateBitmap(resizedBitmap, 270);
				break;
		}
     
      
        
        return resizedBitmap;
	}
	public static Bitmap RotateBitmap(Bitmap bitmap, int degrees)
	{
		Matrix matrix = new Matrix();
		matrix.PostRotate(degrees);
		return Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, matrix, true);
	}

	public enum Orientation
	{
		Undefined = 0,
		Normal = 1,
		FlipHorizontal = 2,
		Rotate180 = 3,
		FlipVertical = 4,
		Transpose = 5,
		Rotate90 = 6,
		Transverse = 7,
		Rotate270  = 8,

	}

    public static int calculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
    {
        // Raw height and width of image
        int height = options.OutHeight;
        int width = options.OutWidth;
        int inSampleSize = 16;

        if (height > reqHeight || width > reqWidth)
        {

            int halfHeight = height / 2;
            int halfWidth = width / 2;

            // Calculate the largest inSampleSize value that is a power of 2 and keeps both
            // height and width larger than the requested height and width.
            while ((halfHeight / inSampleSize) > reqHeight
                   && (halfWidth / inSampleSize) > reqWidth)
            {
                inSampleSize *= 2;
            }
        }

        return inSampleSize;
    }
}
