namespace Stenography.Core.Sound
{
    public interface IHeaderParser
    {
        bool IsValid(byte[] fileBytes);
        HeaderType Parse(byte[] fileBtes);
    }
}
