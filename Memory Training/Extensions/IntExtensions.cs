


using Android.Util;
using Plugin.CurrentActivity;

namespace MemoryTraining.Extensions
{
    internal static class IntExtensions
    {
        public static int ToDp(this int pixelValue)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixelValue,
                CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics);
        }
    }
}