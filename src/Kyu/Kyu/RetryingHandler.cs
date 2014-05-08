using System;
using System.Threading.Tasks;

namespace Kyu
{
    public class RetryingHandler<T> : Handles<T>
        where T : Message
    {
        private readonly Handles<T> _handler;

        public RetryingHandler(Handles<T> handler)
        {
            _handler = handler;
        }

        public bool Handle(T message)
        {
            while (true)
            {
                if (_handler.Handle(message))
                    break;

                Console.WriteLine("Delaying...");
                Task.Delay(1000).Wait();
            }

            return true;
        }
    }
}