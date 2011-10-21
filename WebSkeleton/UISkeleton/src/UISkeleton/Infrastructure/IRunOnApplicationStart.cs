namespace UISkeleton.Infrastructure
{
	using Castle.Windsor;

	public interface IRunOnApplicationStart
	{
		void Start(IWindsorContainer container);
	}
}