namespace Kyu.Bus
{
    public interface Publisher
    {
        void Publish<T>(T message) where T : Event;
        void Execute<T>(T command) where T : Command;
    }
}