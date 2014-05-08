namespace Kyu
{
    public class PublishToTypeHandler<T> : Handles<T> where T : Message
    {
        private readonly TopicBasedDispatcher _dispatcher;

        public PublishToTypeHandler(TopicBasedDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        
        public bool Handle(T message)
        {
            _dispatcher.Publish(message.GetType().Name, message);
            return true;
        }
    }
}