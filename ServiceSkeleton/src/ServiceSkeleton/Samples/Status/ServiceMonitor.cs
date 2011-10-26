namespace ServiceSkeleton.Samples.Status
{
	using System;
	using System.Linq;
	using BclExtensionMethods;
	using Infrastructure.Status;
	using Microsoft.Practices.ServiceLocation;
	using NServiceBus;

	public class ServiceMonitor : IWantToRunAtStartup, IMessageHandler<GetServiceStatus>
	{
		private readonly IBus _Bus;
		public DateTime StartedAt = DateTime.Now;

		public ServiceMonitor(IBus bus)
		{
			_Bus = bus;
		}

		public void Run()
		{
		}

		public void Stop()
		{
		}

		public void Handle(GetServiceStatus message)
		{
			var response = new ServiceStatusResponse {StartedAt = StartedAt};

			ServiceLocator.Current.GetAllInstances<IHaveATaskMonitor>()
				.SelectMany(m => m.TaskMonitor.GetProgress())
				.ForEach(response.Progresses.Add);
			ServiceLocator.Current.GetAllInstances<IHaveStatusToReport>()
				.ForEach(
					settingsProvider =>
					response.Statuses.Add(settingsProvider.GetType().ToString(),
					                      settingsProvider.MyStatuses().Select(s => s.ToString()).ToList()));

			_Bus.Reply(response);
		}
	}
}