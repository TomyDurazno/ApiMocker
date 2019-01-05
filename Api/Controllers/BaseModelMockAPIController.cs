using APIMocker.Configs;
using ServiceCore.Recursos.Store;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace APIMocker.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public abstract class BaseModelMockAPIController<T> : ApiController
    {
        #region Properties

        public ModelStore<T> Store
        {
            get
            {
                var store = new ModelStore<T>(StoreName);
                store.Seed(GetInitial);
                return store;
            }
        }

        private string StoreName { get => Config.StoreName; }

        ///<summary>
        /// Configuration object
        ///</summary>
        public abstract MockModelAPIConfig Config { get; }

        #endregion

        #region Private Methods

        private T GetInitial()
        {
            if (string.IsNullOrEmpty(Config.Seed))
                return Activator.CreateInstance<T>();
            else
                return Seeds.Seeds.FromModel<T>(Config.Seed);
        }

        #endregion
    }
}