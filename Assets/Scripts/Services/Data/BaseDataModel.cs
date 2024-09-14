using System;

namespace Services.Data
{
    public abstract class BaseDataModel
    { 
        public event Action<BaseDataModel> OnDemandSave;
        
        public void DemandSave()
        {
            OnDemandSave?.Invoke(this);
        }
    }
}