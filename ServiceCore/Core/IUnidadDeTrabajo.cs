using System;
using System.Data.Entity;

namespace ServiceCore
{
    public interface IUnidadDeTrabajo : IDisposable
    {
        IRepositorio<T> GetRepositorio<T>() where T : class, IEntidad;
        DbContext DbContext { get; }
        int SaveChanges();

    }

    public class UnidadDeTrabajo : IDisposable, IUnidadDeTrabajo
    {


        private bool disposed;
        private readonly DbContext dbContext;

        public DbContext DbContext
        {
            get
            {
                return this.dbContext;
            }
        }


        // Para unit testing /  inyectar por Ninject
        public UnidadDeTrabajo(DbContext dbContext)
        {
            this.dbContext = dbContext;

        }


        public IRepositorio<T> GetRepositorio<T>() where T : class, IEntidad
        {
            IRepositorio<T> repositorio;

            repositorio = new Repositorio<T>(this.dbContext);

            return repositorio as IRepositorio<T>;
        }

        public int SaveChanges()
        {
            return this.dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.dbContext.Dispose();
                }
            }

            this.disposed = true;
        }

    }
}
