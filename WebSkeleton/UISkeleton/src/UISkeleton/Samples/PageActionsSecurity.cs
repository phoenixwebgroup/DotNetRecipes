namespace UISkeleton.Samples
{
	using System;
	using System.Reflection;
	using HtmlTags.UI.Conventions;

	public class PageActionsSecurity : IPageActionsSecurity
	{
		public bool UserCanAccess(MethodInfo action)
		{
			// todo implement page actions security
			throw new NotImplementedException();
		}
	}
}