using System;
using System.Threading.Tasks;

namespace Pooyan.test.infrastructure.StatesBase
{
    public class StateBase<TEntity> where TEntity:class
    {
        public event Func<TEntity,Task> OnChange;
        public event Func<Task<TEntity>> OnInitializeAsync;
        public event Func<TEntity> OnInitialize;
        public StateBase()
        {
            if (OnInitialize != null)
            {
                CurrentItem = OnInitialize.Invoke();
            }
            else if(OnInitializeAsync!=null)
            {
                CurrentItem = OnInitializeAsync.Invoke().Result;
            }
        }

        public string TypeName => typeof(TEntity).Name.Replace("State","");
        private TEntity _currentItem;

        public TEntity CurrentItem
        {
            get => _currentItem;
            set
            {
                _currentItem = value;
                OnChange?.Invoke(value);
            }
        }
    }
}
