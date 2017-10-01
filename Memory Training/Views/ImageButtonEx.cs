using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace MemoryTraining.Views
{
    public class ImageButtonEx : ImageButton
    {
        protected ImageButtonEx(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public ImageButtonEx(Context context) : base(context)
        {
        }

        public ImageButtonEx(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public ImageButtonEx(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public ImageButtonEx(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public int CurrentResourceId { get; private set; }

        public override void SetImageResource(int resid)
        {
            CurrentResourceId = resid;
            base.SetImageResource(resid);
        }
    }
}
