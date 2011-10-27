namespace ServiceSkeleton.Samples
{
	using GotFour.Windsor;
	using Infrastructure;

	public class EndpointRegistry : ExtendedRegistryBase
	{
		public EndpointRegistry()
		{
			ScanMyAssemblyFor<IConfigureOnStartup>();
		}
	}
}