using APIMocker.Configs;
using APIMocker.Extensiones;
using APIMocker.Models.MockModels;
using ServiceCore.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace APIMocker.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public abstract class MockAPIController<T> : ApiController
    {
        #region Properties

        public Store<T> Store
        {
            get
            {
                var store = new Store<T>(StoreName, IsEquals);
                store.Seed(GetInitial);
                return store;
            }
        }

        private string StoreName { get => Config.StoreName; }

        ///<summary>
        /// Configuration object
        ///</summary>
        public abstract MockApiConfig<T> Config { get; }

        #endregion

        #region Private Methods

        private bool IsEquals(T item1, T item2) => Config.Key(item1) == Config.Key(item2);

        private ICollection<T> GetInitial()
        {
            if(string.IsNullOrEmpty(Config.Seed))
                return new List<T>();           
            else
                return Seeds.Seeds.From<T>(Config.Seed);                            
        }

        private bool Selector(T item, string key) => Config.Key(item).Equals(key);

        #endregion

        #region GET ALL
        /// <summary>
        /// Parameterless 'GET'
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual IEnumerable<T> Get() => Store.GetAll();

        #endregion

        #region GET
        /// <summary>
        /// 'GET' with id parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]        
        public virtual IHttpActionResult Get(string id)
        {
            var model = Store.Get(u => Selector(u, id));

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        #endregion

        #region HEAD (RESEED)

        [HttpHead]
        public virtual IHttpActionResult Reseed()
        {
            Store.Seed(GetInitial, true);

            return Ok("Reseeded");
        }

        #endregion

        #region POST

        // POST: api/Users
        // POST == Add
        [HttpPost]
        public virtual IHttpActionResult Post(T model)
        {                              
            if (Store.Add(model))
            {
                return Ok(Store.Get(u => Selector(u, Config.Key(model))));
            }

            return BadRequest();
        }

        #endregion

        #region PUT

        // PUT: api/Users/5
        // PUT == Update
        [HttpPut]
        public virtual IHttpActionResult Put(T model)
        {
            if (Store.Update(model))
            {
                return Ok(Store.Get(u => Selector(u, Config.Key(model))));
            }

            return NotFound();
        }

        #endregion

        #region DELETE

        // DELETE: api/Users/5        
        [HttpDelete]
        public virtual IHttpActionResult Delete(string id) => Ok(new { result = Store.Remove(u => Selector(u, id)) });

        #endregion

        #region PATCH (RESET)

        [HttpPatch]
        public virtual IHttpActionResult Patch()
        {
            Store.Reset();
            return Ok();
        }

        #endregion
    }
}
