namespace ServiceSkeleton.Samples
{
	using System;
	using Castle.Windsor;
	using GotFour.Windsor;
	using Infrastructure;
	using NServiceBus;
	using log4net.Config;

	public class EndpointConfig : IConfigureThisEndpoint, IWantCustomInitialization
	{
		public void Init()
		{
			SetLoggingLibrary.Log4Net(XmlConfigurator.Configure);
			var container = GetContainer();

			var busConfiguration = Configure
				// todo add your assemblies here to scan for message handlers typeof (Registry).Assembly)
				.With(typeof (EndpointConfig).Assembly)
				.CastleWindsorBuilder(container)
				.BinarySerializer()
				.MsmqSubscriptionStorage()
				.MsmqTransport()
				.IsTransactional(true)
				.UnicastBus()
				// todo optionally specify message handlers
				//.LoadMessageHandlers(new First<BufferMessagesHandler>().AndThen<ServiceBusListener>())
				.CreateBus();

			RunApplicationStartupComponents(container);

			var bus = busConfiguration.Start();
			AddBusSubscriptions(bus);
		}

		private void AddBusSubscriptions(IBus bus)
		{
			// todo optional bus.Subscribe if listening to events from other endpoints
		}

		private static void RunApplicationStartupComponents(IWindsorContainer container)
		{
			var starters = container.ResolveAll<IRunOnApplicationStart>();
			Array.ForEach(starters, i => i.Start(container));
		}

		public static IWindsorContainer GetContainer()
		{
			var locator = new ExtendedContainer();
			var container = locator.Resolve<IWindsorContainer>();
			// todo install registries
			container.Install(new EndpointRegistry());
			return container;
		}
	}
}