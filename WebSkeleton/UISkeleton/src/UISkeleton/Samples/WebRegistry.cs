namespace UISkeleton.Samples
{
	using System.Web.Mvc;
	using System.Web.Mvc.Async;
	using GotFour.Windsor;
	using Infrastructure;
	using JoinedFilter;
	using JoinedFilter.Windsor;

	public class WebRegistry : ExtendedRegistryBase
	{
		public WebRegistry()
		{
			RegisterControllerFactory();
			RegisterControllers();
			RegisterExportFilters();
			RegisterJoinedFilters();
			// todo
			//RegisterSessionBuilders();

			ScanMyAssembly(IsSelfService);

			// todo any other registrations?
		}

		// todo
		//private void RegisterSessionBuilders()
		//{
		//    SessionBuilderBase.WithWebContext();
		//    For<SettlementsSessionBuilder>().LifeStyle.Singleton();
		//}

		private void RegisterJoinedFilters()
		{
			ScanMyAssemblyFor<IJoinedFilter>();
			ScanMyAssemblyFor<IResultFilter>();
			ScanMyAssemblyFor<IActionFilter>();
			ScanMyAssemblyFor<IExceptionFilter>();
			AddComponent<IActionInvoker, LocatorActionInvoker>();
			AddComponent<AsyncControllerActionInvoker, LocatorAsyncActionInvoker>();
			AddComponent<IFilterLocator, WindsorJoinedFilterLocator>();
			AddComponent<IMasterFilterLocator, MasterFilterLocator>();
			AddComponent<IFilterInjector, WindsorFilterInjector>();
		}

		private void RegisterControllerFactory()
		{
			AddComponent<IControllerFactory, WindsorControllerFactory>();
		}

		private void RegisterControllers()
		{
			ScanMyAssemblyFor<Controller>();
		}

		private void RegisterExportFilters()
		{
			AddComponent<IJoinedFilter, ExportFilter>();
			AddComponent<IJoinedFilter, TableFilter>();
		}
	}
}