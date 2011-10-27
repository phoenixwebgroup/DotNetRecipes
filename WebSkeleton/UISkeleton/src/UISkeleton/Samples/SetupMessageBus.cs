namespace UISkeleton.Samples
{
	using Castle.Windsor;
	using Infrastructure;

	public class SetupMessageBus : IConfigureOnStartup
	{
		public void Configure(IWindsorContainer container)
		{
			// todo if you want to use the messaging framework, configure it here
			//    MessageBusConfiguration.ForWeb();
			//    MessagingBusFactory.BusWriter = new WebBusWriter();
		}
	}
}