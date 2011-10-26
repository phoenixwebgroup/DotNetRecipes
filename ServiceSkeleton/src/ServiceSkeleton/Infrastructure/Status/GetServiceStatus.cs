namespace ServiceSkeleton.Infrastructure.Status
{
	using System;
	using NServiceBus;

	[Serializable]
	public class GetServiceStatus : IMessage
	{
	}
}