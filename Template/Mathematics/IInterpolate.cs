namespace EMBC.Mathematics
{
    public interface IInterpolate<TValue> :
        IInterpolateMultiply<TValue>,
        IInterpolateLinear<TValue>,
        IInterpolateBarycentric<TValue>
    {
    }

    public interface IInterpolateMultiply<out TValue>
    {
        TValue InterpolateMultiply(float multiplier);
    }

    public interface IInterpolateLinear<TValue>
    {
        TValue InterpolateLinear(in TValue other, float alpha);
    }

    public interface IInterpolateBarycentric<TValue>
    {
        TValue InterpolateBarycentric(in TValue other0, in TValue other1, Vector3F barycentric);
    }
}
