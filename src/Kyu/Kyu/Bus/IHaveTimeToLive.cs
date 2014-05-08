using System;

namespace Kyu.Bus
{
    public interface IHaveTimeToLive
    {
        DateTime DropTime { get; }
    }
}