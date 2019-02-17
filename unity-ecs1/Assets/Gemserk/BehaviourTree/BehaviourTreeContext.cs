namespace Gemserk.BehaviourTree
{
    public interface BehaviourTreeContext
    {
        // this is the owner object
        void SetOwner(object o);

        void SetManager<T>(T t) where T : class;

        T GetManager<T>() where T : class;
    }
}