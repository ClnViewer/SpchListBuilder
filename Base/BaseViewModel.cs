using System;
using System.ComponentModel;

namespace SpchListBuilder.Base
{
    public abstract class BaseViewModel : BaseNotifyPropertyChanged
    {
        private AsyncOperation __op = AsyncOperationManager.CreateOperation(null);
        public event EventHandler<StringEventArgs> EventError = delegate { };
        public event EventHandler<StringEventArgs> EventRefresh = delegate { };

        protected void Fire_EventError(StringEventArgs o)
        {
            __op.Post(x => EventError(this, (StringEventArgs)x), o);
        }

        protected void Fire_EventRefresh(StringEventArgs o)
        {
            __op.Post(x => EventRefresh(this, (StringEventArgs)x), o);
        }
    }
}
