namespace Core
{
    public interface IMenuPageable
    {
        void Show();
        void SendArgs<T>(T args) where T : struct;
        void Hide();
    }
}
