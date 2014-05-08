namespace Kyu.Bus
{
    public class Bus : Publisher, Subscribable
    {
        readonly TopicBasedDispatcher _dispatcher = new TopicBasedDispatcher();

        public void Publish<T>(T message) where T : Event
        {
            _dispatcher.Publish(message.GetType().Name, message);
            
            if (message is IHaveCorrelationId)
            {
                var m = message as IHaveCorrelationId;
                _dispatcher.Publish(m.CorrelationId, message);
            }
        }

        public void Execute<T>(T command) where T : Command
        {
            _dispatcher.Publish(command.GetType().Name, command);
        }

        public void Subscribe<T>(Handles<T> handler) where T : Message
        {
            _dispatcher.Subscribe(handler);
        }

        public void Subscribe<T>(string correlationId, Handles<T> handler) where T : Message
        {
            _dispatcher.Subscribe(correlationId, handler);
        }
    }
}