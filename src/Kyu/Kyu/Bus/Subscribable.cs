namespace Kyu.Bus
{
    public interface Subscribable
    {
        void Subscribe<T>(Handles<T> handler) where T : Message;
        void Subscribe<T>(string correlationId, Handles<T> handler) where T : Message;
    }
}