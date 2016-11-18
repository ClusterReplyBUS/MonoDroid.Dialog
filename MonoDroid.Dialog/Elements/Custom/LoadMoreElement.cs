using System;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class LoadMoreElement:Element
	{
		
		public string NormalCaption { get; set; }
		public string LoadingCaption { get; set; }
		public Color TextColor { get; set; }
		public Color BackgroundColor { get; set; }
		public event Action<LoadMoreElement> Tapped = null;
		public TypefaceStyle Font;
		TextAlignment alignment = TextAlignment.Center;

		public TextAlignment Alignment
		{
			get { return alignment; }
			set { alignment = value; }
		}

		public LoadMoreElement() : base("")
		{
		}

		public LoadMoreElement(string normalCaption, string loadingCaption, Action<LoadMoreElement> tapped) : this(normalCaption, loadingCaption, tapped, TypefaceStyle.Bold, Color.Black)
		{
		}

		public LoadMoreElement(string normalCaption, string loadingCaption, Action<LoadMoreElement> tapped, TypefaceStyle font, Color textColor) : base("")
		{
			NormalCaption = normalCaption;
			LoadingCaption = loadingCaption;
			Tapped += tapped;
			Font = font;
			TextColor = textColor;
		}

		public override View GetView(Android.Content.Context context, View convertView, ViewGroup parent)
		{
			TextView caption=new TextView(context);
			caption.SetBackgroundColor(Color.Transparent);


			var tf = Typeface.Default;

			caption.Text = NormalCaption;

			caption.SetTextColor(Color.Yellow);
			caption.SetTypeface(tf,TypefaceStyle.Bold);
			caption.TextAlignment = Alignment;
			return base.GetView(context, convertView, parent);
		}

	}
}
