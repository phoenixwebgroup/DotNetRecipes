namespace UISkeleton.Infrastructure
{
	using Castle.Windsor;

	public interface IConfigureOnStartup
	{
		void Configure(IWindsorContainer container);
	}
}