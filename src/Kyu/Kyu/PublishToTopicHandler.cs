namespace Kyu
{
    public class PublishToTopicHandler<T> : Handles<T> where T : Message
    {
        private readonly TopicBasedDispatcher _dispatcher;
        private readonly string[] _topics;

        public PublishToTopicHandler(TopicBasedDispatcher dispatcher, params string[] topics)
        {
            _dispatcher = dispatcher;
            _topics = topics;
        }

        public bool Handle(T message)
        {
            foreach (var topic in _topics)
                _dispatcher.Publish(topic, message);

            return true;
        }
    }
}
