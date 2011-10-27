namespace UISkeleton.Infrastructure
{
	using Castle.Windsor;
	using GotFour.Windsor;
	using GotFour.Windsor.Testing;

	public class ValidateContainerOnStartup : IConfigureOnStartup
	{
		public void Configure(IWindsorContainer container)
		{
			ExtendedContainer.Instance.ExpectAllRegistrationsAreValid();
		}
	}
}