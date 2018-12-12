[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Api.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Api.App_Start.NinjectWebCommon), "Stop")]

namespace Api.App_Start
{
    using System;
    using System.Data.Entity;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using ServiceCore;
    using ServiceCore.Datos;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<DbContext>().To<Modelo>().InRequestScope();

            kernel.Bind<IUnidadDeTrabajo>().To<UnidadDeTrabajo>().InRequestScope();

            //kernel.Bind<IServicio<Usuario>>().To<Servicio<Usuario>>().InRequestScope();

            //kernel.Bind<IServicio<Campania>>().To<Servicio<Campania>>().InRequestScope();

            //kernel.Bind<IServicio<Categoria>>().To<Servicio<Categoria>>().InRequestScope();


            //kernel.Bind<IServicio<Ubicacion>>().To<Servicio<Ubicacion>>().InRequestScope();
        }        
    }
}
