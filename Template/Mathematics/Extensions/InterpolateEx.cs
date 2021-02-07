namespace EMBC.Mathematics.Extensions
{
    public static class InterpolateEx
    {
        public static float InterpolateLinear(this float left, float right, float alpha)
        {
            return left + (right - left) * alpha;
        }
    }
}
