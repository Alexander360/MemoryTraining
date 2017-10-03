


using Plugin.CurrentActivity;

namespace MemoryTraining.Extensions
{
    internal static class IntExtensions
    {
        public static int ToDp(this int pixelValue)
        {
            var dp = (int)(pixelValue / CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics.Density);
            return dp;
        }
    }
}