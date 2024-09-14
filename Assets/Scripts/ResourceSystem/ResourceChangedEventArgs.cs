using System;

namespace ResourceSystem
{
    public class ResourceChangedEventArgs : EventArgs
    {
        public ResourceType ResourceType { get; private set; }
        public double OldValue { get; private set; }
        public double NewValue { get; private set; }

        public ResourceChangedEventArgs(ResourceType resourceType, double oldValue, double newValue)
        {
            ResourceType = resourceType;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
    
}