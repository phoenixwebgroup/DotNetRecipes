namespace ServiceSkeleton.Samples
{
	using Castle.Windsor;
	using GotFour.Windsor;
	using GotFour.Windsor.Testing;
	using Infrastructure;

	public class ValidateContainerOnStartup : IConfigureOnStartup
	{
		public void Configure(IWindsorContainer container)
		{
			ExtendedContainer.Instance.ExpectAllRegistrationsAreValid();
		}
	}
}