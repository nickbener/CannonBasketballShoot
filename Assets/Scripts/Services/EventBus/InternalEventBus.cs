using System;
using System.Collections.Generic;

namespace Services.EventBus
{
    public class InternalEventBus
    {
        private readonly Dictionary<int, EventHandler> _events = new Dictionary<int, EventHandler>();

        public InternalEventBus(IEnumerable<int> keys)
        {
            foreach (int key in keys)
            {
                _events.Add(key, null);
            }
        }
        
        public void AddListener(int eventKey, EventHandler eventHandler)
        {
            _events[eventKey] += eventHandler;
        }
        
        public void Broadcast(int eventKey, object sender, EventArgs args)
        {
            _events[eventKey]?.Invoke(sender, args);
        }
        
        public void RemoveListener(int eventKey, EventHandler eventHandler)
        {
            _events[eventKey] -= eventHandler;
        }
    }
}