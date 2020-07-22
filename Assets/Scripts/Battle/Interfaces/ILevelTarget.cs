namespace Battle.Interfaces
{
    public  interface ILevelTarget<T>
    {
        bool CheckTarget();
        void SetTarget(T target);
        T GetTarget();
    }
}
