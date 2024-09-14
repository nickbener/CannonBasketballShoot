using System;
using System.Linq;
using Services.EventBus;

namespace CodeBase
{
    public class EventBusService
    {
        private readonly InternalEventBus _internalEventBus;

        public EventBusService()
        {
            _internalEventBus = new InternalEventBus(Enum.GetValues(typeof(GameEventKey)).Cast<int>());
        }

        public void AddListener(GameEventKey eventKey, EventHandler eventHandler)
        {
            _internalEventBus.AddListener((int)eventKey, eventHandler);
        }
        
        public void Broadcast(GameEventKey eventKey, object sender = null, EventArgs args = null)
        {
            _internalEventBus.Broadcast((int)eventKey, sender, args);
        }
        
        public void RemoveListener(GameEventKey eventKey, EventHandler eventHandler)
        {
            _internalEventBus.RemoveListener((int)eventKey, eventHandler);
        }
    }
}