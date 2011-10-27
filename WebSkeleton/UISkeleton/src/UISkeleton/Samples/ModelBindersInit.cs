namespace UISkeleton.Samples
{
	using System.Web.Mvc;
	using Castle.Windsor;
	using HtmlTags.UI.ModelBinders;
	using Infrastructure;

	public class ModelBindersInit : ModelBinderRegistryBase, IConfigureOnStartup
	{
		public void Configure(IWindsorContainer container)
		{
			ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());
			DecimalModelBinder();
		}
	}
}