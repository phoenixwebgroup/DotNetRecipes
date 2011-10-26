namespace ServiceSkeleton.Samples
{
	using Castle.Windsor;
	using Infrastructure;

	public class ConfigureMessageBus : IRunOnApplicationStart
	{
		public void Start(IWindsorContainer container)
		{
			// todo if you need a message context, bring the message bus framwork in
			//MessageBusConfiguration.ForService();
			//MessagingBusFactory.BusWriter = new ServiceBusWriter();
		}
	}
}