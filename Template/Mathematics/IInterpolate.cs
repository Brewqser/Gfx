using MathNet.Spatial.Euclidean;

namespace EMBC.Mathematics
{
    #region // single

    public interface IInterpolateSingle<TValue> :
        IInterpolateSingleMultiply<TValue>,
        IInterpolateSingleLinear<TValue>,
        IInterpolateSingleBarycentric<TValue>
    {
    }

    public interface IInterpolateSingleMultiply<out TValue>
    {
        TValue InterpolateMultiply(float multiplier);
    }

    public interface IInterpolateSingleLinear<TValue>
    {
        TValue InterpolateLinear(in TValue other, float alpha);
    }

    public interface IInterpolateSingleBarycentric<TValue>
    {
        TValue InterpolateBarycentric(in TValue other0, in TValue other1, Vector3F barycentric);
    }

    #endregion

    #region // double

    public interface IInterpolateDouble<TValue> :
        IInterpolateDoubleMultiply<TValue>,
        IInterpolateDoubleLinear<TValue>,
        IInterpolateDoubleBarycentric<TValue>
    {
    }

    public interface IInterpolateDoubleMultiply<out TValue>
    {
        TValue InterpolateMultiply(double multiplier);
    }

    public interface IInterpolateDoubleLinear<TValue>
    {
        TValue InterpolateLinear(in TValue other, double alpha);
    }

    public interface IInterpolateDoubleBarycentric<TValue>
    {
        TValue InterpolateBarycentric(in TValue other0, in TValue other1, Vector3D barycentric);
    }

    #endregion
}
