using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCore.Recursos
{
    /// <summary>
    /// In Cache Store. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Store<T>
    {
        #region Properties

        public string Name { get; private set; }

        public int Expires { get; set; }

        #endregion

        #region Item Equality

        Func<T, T, bool> ItemEquals { get; set; }

        #endregion

        #region Constructor

        public Store(string name, Func<T,T,bool> itemEquals, int expiresInMinutes = 60)
        {
            Name = name;
            ItemEquals = itemEquals;
            Expires = expiresInMinutes;
        }

        #endregion

        #region State Management

        private GlobalCache GlobalCache { get => new GlobalCache(Name); }

        public ICollection<T> State 
        {
            get => GlobalCache.GetItem<ICollection<T>>();
            set => GlobalCache.SetItem(value, Expires);
        }

        public void Seed(Func<ICollection<T>> getItems, bool overwrite = false) 
        {
            if(State == null || overwrite)
            {
                State = getItems();
            }
        }

        public void Reset(bool runSeed = true) => GlobalCache.RemoveItem();

        #endregion

        #region CRUD Operations

        public bool Exists(Func<T,bool> predicate) => State.Any(predicate);

        public bool Exists(T item) => State.Any(i => ItemEquals(i, item));

        public T Get(Func<T, bool> selector) => State.FirstOrDefault(selector);

        public ICollection<T> GetAll() => State;

        public bool Add(T item) 
        {
            var state = State;

            if(!state.Any(i => ItemEquals(i, item))) //No one matches predicate
            {
                state.Add(item);
                State = state;
                return true;
            }

            return false;
        }

        public bool Update(T item) 
        {
            var state = State;

            var entity = state.Where(i => ItemEquals(i, item)).FirstOrDefault();

            if(entity != null)
            {
                var newState = state.Where(i => !ItemEquals(i, item))
                                    .ToList();

                newState.Add(item);
                State = newState;
                return true;
            }

            return false;
        }

        public bool Remove(Func<T, bool> selector) 
        {
            var state = State;

            if(state.Any(selector))
            {
                State = state.Where(i => !selector(i)).ToList();
                return true;
            }

            return false;
        }

        #endregion
    }
}
