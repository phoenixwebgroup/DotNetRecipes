namespace UISkeleton.Samples
{
	using System.Web.Mvc;
	using GotFour.Windsor;
	using Infrastructure;

	public class WebRegistry : ExtendedRegistryBase
	{
		public WebRegistry()
		{
			RegisterControllerFactory();
			RegisterControllers();
			// todo
			//RegisterSessionBuilders();

			ScanMyAssembly(IsSelfService);

			// todo any other registrations?
		}

		// todo
		//private void RegisterSessionBuilders()
		//{
		//    SessionBuilderBase.WithWebContext();
		//    For<SettlementsSessionBuilder>().LifeStyle.Singleton();
		//}

		private void RegisterControllerFactory()
		{
			AddComponent<IControllerFactory, WindsorControllerFactory>();
		}

		private void RegisterControllers()
		{
			ScanMyAssemblyFor<Controller>();
		}
	}
}