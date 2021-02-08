namespace EMBC.Drivers.Gdi.Render.Rasterization
{
    public interface IBufferBinding<out TVsIn>
        where TVsIn : unmanaged
    {
        TVsIn GetVsIn(uint index);
    }
}
