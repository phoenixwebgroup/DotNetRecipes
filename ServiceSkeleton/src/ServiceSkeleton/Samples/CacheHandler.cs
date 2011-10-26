namespace ServiceSkeleton.Samples
{
	using System;
	using NServiceBus;
	using log4net;

	public class CacheHandler : IMessageHandler<ClearCaches>
	{
		public void Handle(ClearCaches message)
		{
			LogManager.GetLogger(GetType()).Info("Clearing caches");
			// todo pluging clearing
		}
	}

	[Serializable]
	public class ClearCaches : IMessage
	{
		// todo put in a shared library
	}
}