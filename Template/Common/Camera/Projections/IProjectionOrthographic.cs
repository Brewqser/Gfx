namespace EMBC.Common.Camera.Projections
{
    public interface IProjectionOrthographic :
        IProjection
    { 
        double FieldWidth { get; }

        double FieldHeight { get; }
    }
}
