namespace Kyu
{
    public class NarrowingHandler<TFrom, TTo> : Handles<TTo> where TFrom:TTo
        where TTo : Message
    {
        private readonly Handles<TFrom> _handler;

        public NarrowingHandler(Handles<TFrom> handler)
        {
            _handler = handler;
        }

        public bool Handle(TTo message)
        {
            if(message is TFrom)
                return _handler.Handle((TFrom) message);
            return true;
        }
    }
}