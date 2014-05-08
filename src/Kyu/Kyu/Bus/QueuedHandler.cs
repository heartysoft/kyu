using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Kyu.Bus
{
    public class QueuedHandler<T> : Handles<T> where T : Message
    {
        public string queueId { get; set; }
        private readonly Handles<T> _next;
        private ConcurrentQueue<T> _orders = new ConcurrentQueue<T>();
        public CancellationTokenSource Token = new CancellationTokenSource();
        private readonly int _maxSize;

        public QueuedHandler(string queueId, Handles<T> next, int maxSize)
        {
            this.queueId = queueId;
            _next = next;
            _maxSize = maxSize;
        }

        public QueuedHandler(string queueId, Handles<T> next)
            : this(queueId, next, 3)
        {

        }

        public bool Handle(T message)
        {
            if (_orders.Count >= _maxSize)
                return false;

            _orders.Enqueue(message);
            return true;
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                while (Token.IsCancellationRequested == false)
                {
                    T order;
                    if (_orders.TryDequeue(out order))
                        _next.Handle(order);
                    else
                        Task.Delay(1000).Wait();
                }
            }, TaskCreationOptions.LongRunning);
        }

        public QueueInfo GetQueueInfo()
        {
            return new QueueInfo(queueId, _orders.Count);
        }

        public struct QueueInfo
        {
            public string QueueId { get; private set; }
            public int BacklogSize { get; private set; }

            public QueueInfo(string queueId, int backlogSize)
                : this()
            {
                QueueId = queueId;
                BacklogSize = backlogSize;
            }

            public override string ToString()
            {
                return string.Format("{0}\t{1}", QueueId, BacklogSize);
            }
        }
    }
}