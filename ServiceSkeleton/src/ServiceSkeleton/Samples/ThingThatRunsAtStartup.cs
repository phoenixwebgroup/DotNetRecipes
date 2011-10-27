namespace ServiceSkeleton.Samples
{
	using NServiceBus;
	
	// todo rename, this will be scanned in by NServiceBus if you add this assembly to it's With configuration

	public class ThingThatRunsAtStartup : IWantToRunAtStartup
	{
		public void Run()
		{
			// todo add behavior or delete this
		}

		public void Stop()
		{
		}
	}
}