namespace ServiceSkeleton.Infrastructure.Status
{
	using System;
	using System.Collections.Generic;
	using NServiceBus;

	[Serializable]
	public class ServiceStatusResponse : IMessage
	{
		public DateTime StartedAt { get; set; }

		public IList<AsyncTaskProgress> Progresses { get; protected set; }

		public IDictionary<string, IEnumerable<string>> Statuses { get; protected set; }

		public ServiceStatusResponse()
		{
			Progresses = new List<AsyncTaskProgress>();
			Statuses = new Dictionary<string, IEnumerable<string>>();
		}

		public double DaysUp()
		{
			return DateTime.Now.Subtract(StartedAt).TotalDays;
		}
	}
}