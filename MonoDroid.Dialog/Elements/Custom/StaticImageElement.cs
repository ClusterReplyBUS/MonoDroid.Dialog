using System;
using Android.Graphics;

namespace MonoDroid.Dialog
{
	public class StaticImageElement:ImageElement
	{
		public StaticImageElement(string caption,Bitmap image, int height, int width ) : base(image)
		{


		}

		public static Bitmap MakeTransparent(int width, int height)
		{

			return BitmapFactory.DecodeByteArray(new byte[] { },0,0);
		}
	}
}
