namespace UISkeleton.Infrastructure
{
	using System.Web.Mvc;
	using Castle.Windsor;

	public class SetupControllerFactory : IConfigureOnStartup
	{
		public void Configure(IWindsorContainer container)
		{
			var factory = container.Resolve<IControllerFactory>();
			ControllerBuilder.Current.SetControllerFactory(factory);
		}
	}
}