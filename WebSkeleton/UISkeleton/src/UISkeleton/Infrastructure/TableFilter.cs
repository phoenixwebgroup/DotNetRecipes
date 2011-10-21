namespace UISkeleton.Infrastructure
{
	using System.Web.Mvc;
	using HtmlTags.UI.TableResult;
	using JoinedFilter;

	public class TableFilter : TableFilterAttribute, IJoinedFilter
	{
		public bool JoinsTo(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
		{
			var method = actionDescriptor.TryGetMethodInfo();
			return method != null && method.ReturnType == typeof (TableResult);
		}
	}
}