using System;
using System.IO;

using Android.Graphics;
using Android.Media;

public static class BitmapHelpers
{
	public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
	{
		// First we get the the dimensions of the file on disk
		BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
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
		options.InSampleSize = inSampleSize;
		options.InJustDecodeBounds = false;
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
}
