using ServiceCore.Recursos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCore
{
    public interface IServicio<T> where T : class, IEntidad
    {
        T Traer(int id);

        T Traer(string id);

        IQueryable<T> BuscarTodos(Boolean eliminados = false);
        void Crear(T item);
        void Modificar(T item);
        void Modificar(T entity, string[] camposModificados);

        void Eliminar(int id);
        void Eliminar(string id);

        int SaveChanges();

    }

    public class Servicio<T> : IServicio<T> where T : class, IEntidad
    {
        #region Propiedades


        protected readonly IRepositorio<T> _repositorio;
        protected readonly IUnidadDeTrabajo _unitOfWork;
        //protected readonly IExceptionManager _exceptionMgr;

        #endregion

        #region constructor

        public Servicio(IUnidadDeTrabajo unitOfWork)
        {

            _unitOfWork = unitOfWork;
            _repositorio = _unitOfWork.GetRepositorio<T>();
        }



        #endregion

        #region Metodos Publicos

        public T Traer(int id)
        {

            T entity;

            try
            {
                entity = _repositorio.Traer(id);


            }
            catch (Exception ex)
            {
                //throw _exceptionMgr.HandleException("Ocurrió un error al obtener el ítem", ex);
                entity = default(T);
            }
            return entity;

        }

        public T Traer(string id)
        {

            T entity;

            try
            {
                entity = _repositorio.Traer(id);


            }
            catch (Exception ex)
            {
                // throw _exceptionMgr.HandleException("Ocurrió un error al obtener el ítem", ex);
                entity = default(T);
            }

            return entity;

        }


        public IQueryable<T> BuscarTodos(Boolean eliminados = false)
        {
            try
            {
                var entities = eliminados ? _repositorio.TraerTodos() : _repositorio.TraerTodos().Where(e => !e.Eliminado);

                return entities;
            }
            catch (Exception ex)
            {
                //throw _exceptionMgr.HandleException("Ocurrió un error al obtener los ítems", ex);
                throw ex;
            }
        }

        //public async Task<Wrapper<T>> BuscarUno(Expression<Func<T, bool>> exp)
        //{
        //    return (await BuscarTodos().Where(exp).FirstOrDefaultAsync()).ToWrapper();
        //}

        public virtual void Crear(T entity)
        {
            try
            {
                // var operacion = ContextoVerificacion.Operaciones.Crear;
                // VerificarAcceso(entity, operacion);
                entity.FechaCreacion = DateTime.Now;
                entity.FechaEdicion = DateTime.Now;
                _repositorio.Crear(entity);
                _unitOfWork.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                // throw _exceptionMgr.HandleException("Ocurrió un error al crear el ítem", ex);
            }
        }

        //public virtual bool? Crear(T entity, bool persistent = true, string nom = "juan")
        //{
        //    entity.FechaCreacion = DateTime.Now;
        //    entity.FechaEdicion = DateTime.Now;
        //    _repositorio.Crear(entity);

        //    //persistent
        //    //  return persistent.IfTrue(() => _unitOfWork.SaveChanges());

        //    return true;
        //}

        public virtual void Crear(T entity, bool verificarAcceso = true)
        {
            try
            {

                entity.FechaCreacion = DateTime.Now;
                entity.FechaEdicion = DateTime.Now;
                _repositorio.Crear(entity);
                _unitOfWork.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual void Modificar(T entity)
        {
            try
            {
                entity.FechaEdicion = DateTime.Now;
                _repositorio.Modificar(entity);
                _unitOfWork.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void Modificar(T entity, string[] camposModificados)
        {
            try
            {
                var entityActual = Traer(entity.GetId());
                this.Modificar(entityActual, entity, camposModificados);
            }
            catch (Exception ex)
            {

            }
        }

        public virtual void Modificar(T entity, string[] camposModificados, string guid)
        {
            try
            {
                var entityActual = Traer(guid);
                this.Modificar(entityActual, entity, camposModificados);
            }
            catch (Exception ex)
            {
                // throw _exceptionMgr.HandleException("Ocurrió un error al editar el ítem", ex);
            }
        }

        public virtual void Modificar(T entityActual, T entityModificada, string[] camposModificados, bool noMoficarFechaEdicion = false, bool esWs = false)
        {
            try
            {

                if (!noMoficarFechaEdicion)
                {
                    var index = camposModificados.Length;
                    Array.Resize(ref camposModificados, index + 1);
                    camposModificados[index] = "FechaEdicion";
                    entityModificada.FechaEdicion = DateTime.Now;
                }
                //PropertyCopy para obligar a cargar la entidad actual y evitar un  error
                //  PropertyCopy.Copy(entityActual, entityActual);
                entityActual.ReinicializarLista(camposModificados);
                _repositorio.Modificar(entityActual);
                PropertyCopy.Copy(entityModificada, entityActual, camposModificados);
                _unitOfWork.SaveChanges();


            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                // throw _exceptionMgr.HandleException("Ocurrió un error al editar el ítem", ex);
            }
        }

        public virtual void Eliminar(int id)
        {
            try
            {
                var entity = _repositorio.Traer(id);

                entity.FechaEdicion = DateTime.Now;
                entity.Eliminado = true;
                _repositorio.Modificar(entity);

                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                //throw _exceptionMgr.HandleException("Ocurrió un error al eliminar el ítem", ex);
            }
        }

        public virtual void Eliminar(T entity)
        {
            try
            {
                entity.FechaEdicion = DateTime.Now;
                entity.Eliminado = true;
                var entityActual = Traer(entity.GetId());

                _repositorio.Modificar(entityActual);
                // PropertyCopy.Copy(entity, entityActual);
                entityActual.Eliminado = true;
                _unitOfWork.SaveChanges();

            }
            catch (Exception ex)
            {
                // throw _exceptionMgr.HandleException("Ocurrió un error al eliminar el ítem", ex);
            }
        }


        public virtual void Eliminar(string id)
        {
            try
            {
                var entity = _repositorio.Traer(id);
                entity.Eliminado = true;
                _repositorio.Modificar(entity);

                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                //throw _exceptionMgr.HandleException("Ocurrió un error al eliminar el ítem", ex);
            }
        }

        public int SaveChanges()
        {
            return _unitOfWork.SaveChanges();
        }

        #endregion
    }
}
