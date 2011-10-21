namespace UISkeleton.Infrastructure
{
	using System.Reflection;
	using System.Web.Mvc;
	using System.Web.Mvc.Async;

	public static class ActionDescriptorExtensions
	{
		public static MethodInfo TryGetMethodInfo(this ActionDescriptor actionDescriptor)
		{
			if (actionDescriptor is ReflectedActionDescriptor)
			{
				return (actionDescriptor as ReflectedActionDescriptor).MethodInfo;
			}
			if (actionDescriptor is ReflectedAsyncActionDescriptor)
			{
				return (actionDescriptor as ReflectedAsyncActionDescriptor).AsyncMethodInfo;
			}
			return null;
		}
	}
}