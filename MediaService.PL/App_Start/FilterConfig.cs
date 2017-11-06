using System.Web.Mvc;
using MediaService.PL.Utils.Attributes.ErrorHandler;

namespace MediaService.PL
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AiHandleErrorAttribute());
        }
    }
}

//var success = false;
//var startApplication = "Application started running";
//// Note: A single instance of telemetry client is sufficient to track multiple telemetry items.
//var ai = new TelemetryClient();
//ai.TrackTrace(startApplication, SeverityLevel.Information, properties); //Properties can be custom defined
//try
//{
//success = dependency.Call(); //The call to remote dependencies
//}
//catch(Exception e)
//{
//ai.TrackTrace(e.Message, SeverityLevel.Warning, properties);
//}
