using System;
using MathNet.Numerics.LinearAlgebra;

namespace EMBC.Common.Camera.Projections
{
    public interface IProjection :
        ICloneable
    {
        double NearPlane { get; }

        double FarPlane { get; }

        Matrix<double> GetMatrixProjection();

        IProjection GetAdjustedProjection(double aspectRatio);
    }
}
