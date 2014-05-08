using System.Runtime.InteropServices;

namespace Kyu.Bus
{
    public class Queue<T>
    {
        readonly T[] _buffer;
        private readonly int _size;
        private readonly int _mask;

        private Marker _head;
        private Marker _tail;

        public Queue(int sizeAsPowerOfTwo)
        {
            _size = 1 << sizeAsPowerOfTwo;
            _buffer = new T[_size];
            _mask = _size - 1;
        }

        [StructLayout(LayoutKind.Sequential, Size = 64)]
        public struct Marker
        {
            public int Index;
        }

        public bool Offer(T item)
        {
            if (_tail.Index - _head.Index >= _size)
            {
                return false;
            }

            _buffer[_tail.Index & _mask] = item;
            return true;
        }
    }
}