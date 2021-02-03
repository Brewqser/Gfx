namespace EMBC.Common.Camera.Projections
{
    public interface IProjectionPerspective :
        IProjection
    {
        double FieldOfViewY { get; }

        double AspectRatio { get; }
    }
}
