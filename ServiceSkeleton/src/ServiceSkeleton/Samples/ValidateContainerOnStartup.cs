namespace ServiceSkeleton.Samples
{
	using Castle.Windsor;
	using GotFour.Windsor;
	using GotFour.Windsor.Testing;
	using Infrastructure;

	public class ValidateContainerOnStartup : IRunOnApplicationStart
	{
		public void Start(IWindsorContainer container)
		{
			ExtendedContainer.Instance.ExpectAllRegistrationsAreValid();
		}
	}
}