#region usings

using System;
using SimpleInjector;

#endregion

namespace MediaService_BLL_Job.DependencyInjection
{
    /// <summary>
    ///     Provides an abstraction for WebJob functions, enabling dependency injection.
    ///     Mandatory to have on all triggered functions for proper disposal of execution scope.
    /// </summary>
    public abstract class FunctionBase : IDisposable
    {
        private Scope containerScope;

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Sets the execution scope for this <see cref="FunctionBase" />
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
    }
}