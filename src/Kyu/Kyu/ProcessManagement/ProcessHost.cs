using System;
using System.Collections.Generic;
using Kyu.Bus;

namespace Kyu.ProcessManagement
{
    public abstract class ProcessHost
    {
        protected readonly Publisher Bus;
        protected readonly Dictionary<Guid, ProcessManager> InProgress = new Dictionary<Guid, ProcessManager>();
        protected ProcessHost(Publisher bus)
        {
            Bus = bus;
        }

        protected bool DispatchToRegisteredProcessManager<T>(Guid correlationId, T message) where T : Message
        {
            ProcessManager manager;
            if (InProgress.TryGetValue(correlationId, out manager))
            {
                var x = manager as Handles<T>;
                if (x != null)
                {
                    // message received for a dead request?
                    x.Handle(message);
                }
            }

            return true;
        }
    }
}