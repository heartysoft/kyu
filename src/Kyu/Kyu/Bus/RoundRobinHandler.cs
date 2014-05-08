namespace Kyu.Bus
{
    public class RoundRobinHandler<T> : Handles<T> where T : Message
    {
        private readonly QueuedHandler<T>[] _handlers;
        private int _currentIndex;

        public RoundRobinHandler(params QueuedHandler<T>[] handlers)
        {
            _handlers = handlers;
        }

        public bool Handle(T message)
        {
            for (int i = 0; i < _handlers.Length; i++)
            {
                if (_handlers[_currentIndex].Handle(message))
                {
                    incrementIndex();
                    return true;
                }

                incrementIndex();
            }

            return false;
        }

        private void incrementIndex()
        {
            _currentIndex++;
            if (_currentIndex == _handlers.Length)
                _currentIndex = 0;
        }
    }
}