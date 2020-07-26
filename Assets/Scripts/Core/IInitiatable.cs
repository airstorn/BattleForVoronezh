namespace Core
{
    public interface IInitiatable<T>
    {
        void Init(T initiationObject);
    }
}