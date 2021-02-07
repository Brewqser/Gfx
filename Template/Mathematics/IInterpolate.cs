namespace EMBC.Mathematics
{
    public interface IInterpolate<TValue> :
        IInterpolateLinear<TValue>
    {
    }

    public interface IInterpolateLinear<TValue>
    {
        TValue InterpolateLinear(in TValue other, float alpha);
    }
}
