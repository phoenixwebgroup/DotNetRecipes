namespace UISkeleton.Infrastructure
{
	using System.Web.Mvc;

	public class SetJsonRequestBehaviorGlobally : IResultFilter
	{
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