namespace Gemserk.BehaviourTree
{
    public interface BehaviourTreeContext
    {
        void SetObject(object o);

        void SetManager<T>(T t) where T : class;

        T GetManager<T>() where T : class;
    }
}