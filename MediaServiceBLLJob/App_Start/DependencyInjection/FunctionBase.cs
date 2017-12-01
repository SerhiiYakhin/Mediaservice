using System;
using System.Threading.Tasks;
using SimpleInjector;

namespace MediaServiceBLLJob.App_Start.DependencyInjection
{
    /// <summary>
    /// Provides an abstraction for WebJob functions, enabling dependency injection. 
    /// Mandatory to have on all triggered functions for proper disposal of execution scope.
    /// </summary>
    public abstract class FunctionBase : IDisposable
    {
        private Scope containerScope;

        /// <summary>
        /// Initializes a new instance of <see cref="FunctionBase"/>.
        /// </summary>
        protected FunctionBase() { }

        /// <summary>
        /// Sets the execution scope for this <see cref="FunctionBase"/>
        /// </summary>
        /// <param name="scope">The execution scope.</param>
        public void SetScope(Scope scope)
        {
            containerScope = scope;
        }

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                containerScope?.Dispose();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
