namespace Kyu.Bus
{
    public interface Handles<in T> where T : Message
    {
        bool Handle(T message);
    }
}