namespace UISkeleton.Samples
{
	using System.Web;
	using System.Web.Mvc;
	using BclExtensionMethods;
	using Castle.MicroKernel.Registration;
	using Castle.Windsor;
	using GotFour.Windsor;
	using GotFour.Windsor.Testing;
	using HtmlTags.UI.Helpers;
	using Spark.Web.Mvc;
	using Infrastructure;

	public class WebEndpoint : HttpApplication
	{
		protected void Application_Start()
		{
			Container = new ExtendedContainer();
			SetupComponents(Container);
			SetControllerFactory(Container);
			// todo
			//SetupMessageBus();
			ViewEngines.Engines.Add(new SparkViewFactory());
			HtmlContentExtensions.DefaultScriptLocation = "~/Scripts/";
			HtmlContentExtensions.DefaultStyleSheetLocation = "~/Content/css/";
		}

		// todo if you want to use the messaging framework, configure it here
		//private void SetupMessageBus()
		//{
		//    MessageBusConfiguration.ForWeb();
		//    MessagingBusFactory.BusWriter = new WebBusWriter();
		//}
		
		protected ExtendedContainer Container
		{
			get { return (Application.Get("ServiceLocator") as ExtendedContainer); }
			set { Application.Set("ServiceLocator", value); }
		}

		protected virtual void SetupComponents(IWindsorContainer container)
		{
			LoadRegistrations(container);
			ScanIn<IRunOnApplicationStart>(container);
			Init(container);
			ExtendedContainer.Instance.ExpectAllRegistrationsAreValid();
		}

		private void SetControllerFactory(IWindsorContainer container)
		{
			var factory = container.Resolve<IControllerFactory>();
			ControllerBuilder.Current.SetControllerFactory(factory);
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

		public void Init(IWindsorContainer container)
		{
			container.ResolveAll<IRunOnApplicationStart>()
				.ForEach(i => i.Start(container));

			new HtmlConventions().Start(container);
		}

		public void LoadRegistrations(IWindsorContainer container)
		{
			ScanIn<IWindsorInstaller>(container);
			LoadRegistries(container);
		}
	}
}