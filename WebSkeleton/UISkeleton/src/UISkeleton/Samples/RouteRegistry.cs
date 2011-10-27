namespace UISkeleton.Samples
{
	using System.Web.Mvc;
	using System.Web.Routing;
	using Castle.Windsor;
	using Infrastructure;

	public class RouteRegistry : IConfigureOnStartup
	{
		protected static void IgnoreAxds(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
		}

		protected static void IgnoreFavicons(RouteCollection routes)
		{
			routes.IgnoreRoute("{*favicon}", new {favicon = @"(.*/)?favicon.ico(/.*)?"});
		}

		private static void RegisterRoutes(RouteCollection routes)
		{
			IgnoreAxds(routes);
			IgnoreFavicons(routes);
			routes.IgnoreRoute("Content/{*pathInfo}");
			routes.IgnoreRoute("Scripts/{*pathInfo}");
			
			// todo set default controller & action below 
			var defaultControllerName = "Home";
			var defaultActionName = "Index";

			routes.MapRoute(
				"IdLess",
				"{controller}/{action}",
				new { controller = defaultControllerName, action = defaultActionName }
				);

			routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
				new { controller = defaultControllerName, action = defaultActionName, id = "" }
				);
		}

		public void Configure(IWindsorContainer container)
		{
			var routes = RouteTable.Routes;
			RegisterRoutes(routes);
		}
	}
}