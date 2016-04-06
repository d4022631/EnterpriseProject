using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BookingBlock.WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ModelBinderProviders.BinderProviders.Add(new EFModelBinderProvider());


            SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));

            AreaRegistration.RegisterAllAreas();
            // GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }

    public class DbGeographyModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            string[] latLongStr = valueProviderResult.AttemptedValue.Split(',');
            string point = string.Format("POINT ({0} {1})", latLongStr[1], latLongStr[0]);
            //4326 format puts LONGITUDE first then LATITUDE
            DbGeography result = valueProviderResult == null ? null :
                DbGeography.FromText(point, 4326);
            return result;
        }
    }

    public class EFModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (modelType == typeof(DbGeography))
            {
                return new DbGeographyModelBinder();
            }
            return null;
        }
    }
}
