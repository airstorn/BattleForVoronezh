namespace Battle.Interfaces
{
    public  interface ILevelTarget<T>
    {
        bool CheckTarget();
        ILevelTarget<T> SetTarget(T target);
        T GetTarget();
    }
}
