namespace UISkeleton.Samples
{
	using System.Web;
	using System.Web.Mvc;
	using BclExtensionMethods;
	using Castle.MicroKernel.Registration;
	using Castle.Windsor;
	using GotFour.Windsor;
	using HtmlTags.UI.Helpers;
	using Infrastructure;
	using Spark.Web.Mvc;

	public class WebEndpoint : HttpApplication
	{
		protected void Application_Start()
		{
			Container = new ExtendedContainer();
			LoadRegistrations(Container);
			ScanIn<IConfigureOnStartup>(Container);
			RunStartupConfigurations(Container);
			ViewEngines.Engines.Add(new SparkViewFactory());
			HtmlContentExtensions.DefaultScriptLocation = "~/Scripts/";
			HtmlContentExtensions.DefaultStyleSheetLocation = "~/Content/css/";
		}

		protected ExtendedContainer Container
		{
			get { return (Application.Get("ServiceLocator") as ExtendedContainer); }
			set { Application.Set("ServiceLocator", value); }
		}

		private static void LoadRegistries(IWindsorContainer container)
		{
			container
				.ResolveAll<IWindsorInstaller>()
				.ForEach(r => container.Install(r));
		}

		public void ScanIn<T>(IWindsorContainer container)
		{
			container.Register(AllTypes.FromAssemblyContaining<WebRegistry>().BasedOn<T>());
			// todo reference other projects you want to scan in
			//container.Register(AllTypes.FromAssemblyContaining<TypeInAnotherProjectToScan>().BasedOn<T>());
		}

		public void RunStartupConfigurations(IWindsorContainer container)
		{
			container.ResolveAll<IConfigureOnStartup>()
				.ForEach(i => i.Configure(container));
		}

		public void LoadRegistrations(IWindsorContainer container)
		{
			ScanIn<IWindsorInstaller>(container);
			LoadRegistries(container);
		}
	}
}