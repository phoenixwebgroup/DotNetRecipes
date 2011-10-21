namespace UISkeleton.Infrastructure
{
	using System.Web.Mvc;
	using JoinedFilter;

	public class SetJsonRequestBehaviorGlobally : IJoinedFilter, IResultFilter
	{
		public bool JoinsTo(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
		{
			return true;
		}

		public void OnResultExecuting(ResultExecutingContext filterContext)
		{
			if (filterContext.Result is JsonResult)
			{
				(filterContext.Result as JsonResult).JsonRequestBehavior = JsonRequestBehavior.AllowGet;
			}
		}

		public void OnResultExecuted(ResultExecutedContext filterContext)
		{
		}
	}
}