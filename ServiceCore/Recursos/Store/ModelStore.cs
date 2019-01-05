using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCore.Recursos.Store
{
    public class ModelStore<T>
    {
        #region Properties

        public string Name { get; private set; }

        public int Expires { get; set; }

        #endregion

        #region State Management

        private GlobalCache GlobalCache { get => new GlobalCache(Name); }

        public T State
        {
            get => GlobalCache.GetItem<T>();
            set => GlobalCache.SetItem(value, Expires);
        }

        public void Seed(Func<T> getModel, bool overwrite = false)
        {
            if (State == null || overwrite)
            {
                State = getModel();
            }
        }

        public void Reset(bool runSeed = true) => GlobalCache.RemoveItem();

        #endregion

        public ModelStore(string name, int expiresInMinutes = 60)
        {
            Name = name;
            Expires = expiresInMinutes;
        }
    }
}
