namespace UISkeleton.Samples
{
	using Castle.Windsor;
	using HtmlTags.UI.Helpers;
	using Infrastructure;

	public class ConfigureConventionsContentPathing : IConfigureOnStartup
	{
		public void Configure(IWindsorContainer container)
		{
			HtmlContentExtensions.DefaultScriptLocation = "~/Scripts/";
			HtmlContentExtensions.DefaultStyleSheetLocation = "~/Content/css/";
		}
	}
}