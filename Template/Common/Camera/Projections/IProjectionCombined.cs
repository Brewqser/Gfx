namespace EMBC.Common.Camera.Projections
{
    public interface IProjectionCombined :
        IProjection
    {
        IProjection Projection0 { get; }

        IProjection Projection1 { get; }

        double Weight0 { get; }

        double Weight1 { get; }
    }
}
