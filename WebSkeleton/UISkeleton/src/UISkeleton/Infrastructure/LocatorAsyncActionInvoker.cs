namespace UISkeleton.Infrastructure
{
	using System.Web.Mvc;
	using System.Web.Mvc.Async;
	using JoinedFilter;

	public class LocatorAsyncActionInvoker : AsyncControllerActionInvoker
	{
		public LocatorAsyncActionInvoker(IFilterInjector filterInjector, IMasterFilterLocator masterFilterLocator)
		{
			MasterFilterLocator = masterFilterLocator;
			FilterInjector = filterInjector;
		}

		protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
		{
			var filters = base.GetFilters(controllerContext, actionDescriptor);
			FilterInjector.BuildUp(filters);
			MasterFilterLocator.AddComposedFilters(controllerContext, actionDescriptor, filters);
			return filters;
		}

		public IFilterInjector FilterInjector { get; set; }
		public IMasterFilterLocator MasterFilterLocator { get; set; }
	}
}