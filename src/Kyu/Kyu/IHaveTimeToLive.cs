using System;

namespace Kyu
{
    public interface IHaveTimeToLive
    {
        DateTime DropTime { get; }
    }
}