﻿#region usings

using System;
using MediaService.PL;
using Microsoft.Owin;
using NLog;
using Owin;

#endregion

[assembly: OwinStartup(typeof(Startup))]

namespace MediaService.PL
{
    public partial class Startup
    {
        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public void Configuration(IAppBuilder app)
        {
            try
            {
                ConfigureNinject();
                ConfigureAuth(app);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
#if DEBUG
                throw new Exception("", ex);
#else
                app.Use(new FailedSetupMiddleware(ex).Invoke);
#endif
            }
        }
    }
}