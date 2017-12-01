#region usings

using Microsoft.Azure.WebJobs.Host;
using SimpleInjector;

#endregion

namespace MediaService_BLL_Job.DependencyInjection
{
    /// <summary>
    ///     An activator that uses <see cref="SimpleInjector" /> to return instance of a job type.
    /// </summary>
    public class JobActivator : IJobActivator
    {
        /// <summary>
        ///     The container where dependencies are registered.
        /// </summary>
        private readonly Container container;

        /// <summary>
        ///     Creates a new <see cref="JobActivator" /> instance.
        /// </summary>
        /// <param name="container"></param>
        public JobActivator(Container container)
        {
            this.container = container;
        }

        /// <summary>
        ///     Creates a new instance of a webjob function class within it's own execution context scope.
        /// </summary>
        /// <typeparam name="T">The type of the webjob function class being created.</typeparam>
        /// <returns>The newly created instance of a webjob function.</returns>
        public T CreateInstance<T>()
        {
            //var scope = container.BeginExecutionContextScope();
            var scope = SimpleInjector.Lifestyles.AsyncScopedLifestyle.BeginScope(container);

            var returnClass = (T) container.GetInstance(typeof(T));

            if (returnClass is FunctionBase)
            {
                (returnClass as FunctionBase).SetScope(scope);
            }

            return returnClass;
        }
    }
}