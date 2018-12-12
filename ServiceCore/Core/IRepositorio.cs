using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCore
{
    public interface IRepositorio<T> where T : class
    {
        T Traer(int id);
        T Traer(string id);
        IQueryable<T> TraerTodos();
        void Crear(T entity);
        void Modificar(T entity);
        void Eliminar(T entity);
    }

    public class Repositorio<T> : IRepositorio<T> where T : class, IEntidad
    {
        private readonly DbContext dbContext;
        public Repositorio(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual T Traer(int id)
        {
            var entity = this.dbContext.Set<T>().Find(id);

            return entity;
        }


        public virtual T Traer(string id)
        {
            var entity = this.dbContext.Set<T>().Find(id);

            return entity;
        }


        public IQueryable<T> TraerTodos()
        {
            return this.dbContext.Set<T>();
        }

        public virtual void Crear(T entity)
        {
            this.dbContext.Set<T>().Add(entity);
        }

        public virtual void Modificar(T entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Eliminar(T entity)
        {
            if (this.dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.dbContext.Set<T>().Attach(entity);
            }
            this.dbContext.Set<T>().Remove(entity);
        }
    }
}
