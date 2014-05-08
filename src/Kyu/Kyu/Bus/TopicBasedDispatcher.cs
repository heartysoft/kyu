using System;
using System.Collections.Generic;
using System.Linq;

namespace Kyu.Bus
{
    public class TopicBasedDispatcher
    {
        readonly Dictionary<string, List<Handles<Message>>> _handlers = new Dictionary<string, List<Handles<Message>>>();
        
        public void Publish(string topic, Message e)
        {
            var list = getList(topic);

            foreach (var handlesOrder in list)
            {
                handlesOrder.Handle(e);
            }

            if (e is IHaveCorrelationId)
            {
                var correlationId = (e as IHaveCorrelationId).CorrelationId;
                var list2 = getList(correlationId);
                foreach (var handlesOrder in list2)
                {
                    handlesOrder.Handle(e);
                }
            }
        }

        private IEnumerable<Handles<Message>> getList(string topic)
        {
            if (_handlers.ContainsKey(topic))
                return _handlers[topic].ToArray();

            return new Handles<Message>[0];
        }

        private readonly object _lock = new Object();
        public void Subscribe<T>(string topic, Handles<T> handler) where T:Message
        {
            lock (_lock)
            {
                if (_handlers.ContainsKey(topic) == false)
                    _handlers[topic] = new List<Handles<Message>>();

                if (typeof (Command).IsAssignableFrom(typeof (T)))
                {
                    var handlers = getList(topic);
                    if (handlers.Count() != 0)
                        throw new CommandCanOnlyHaveOneHandlerException(topic, typeof (T));
                }

                var narrowing = new NarrowingHandler<T, Message>(handler);
                _handlers[topic].Add(narrowing);
            }
        }

        public class CommandCanOnlyHaveOneHandlerException : Exception
        {
            public CommandCanOnlyHaveOneHandlerException(string topic, Type type)
                : base(string.Format("An attempt was made to register multiple handlers on topic {0} for command type{1}.", topic, type))
            {
            }
        }

        public void Subscribe<T>(Handles<T> handler) where T : Message
        {
            var type = typeof (T).Name;
            Subscribe(type, handler);
        }
    }
}