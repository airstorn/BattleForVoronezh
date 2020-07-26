namespace Core
{
    public interface IResourceListener<T, in TT> where T : IResourcable<TT> where TT : struct
    {
        void UpdateData(TT data);
    }
}
