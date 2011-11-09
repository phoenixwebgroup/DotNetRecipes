namespace UISkeleton.Infrastructure
{
	using System.Web.Mvc;
	using BclExtensionMethods;
	using Castle.Windsor;
	using GotFour.Windsor;

	public class FilterRegistry : ExtendedRegistryBase, IConfigureOnStartup
	{
		public FilterRegistry()
		{
			ScanMyAssemblyFor<IResultFilter>();
			ScanMyAssemblyFor<IActionFilter>();
			ScanMyAssemblyFor<IExceptionFilter>();
			ScanMyAssemblyFor<IAuthorizationFilter>();
		}

		public void Configure(IWindsorContainer container)
		{
			Add<IResultFilter>(container);
			Add<IActionFilter>(container);
			Add<IExceptionFilter>(container);
			Add<IAuthorizationFilter>(container);
		}

		private static void Add<T>(IWindsorContainer container)
			where T : class
		{
			container.ResolveAll<T>()
				.ForEach(Add);
		}

		private static void Add(object filter)
		{
			GlobalFilters.Filters.Add(filter);
		}
	}
}