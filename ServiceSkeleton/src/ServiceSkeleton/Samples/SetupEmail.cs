namespace ServiceSkeleton.Samples
{
	using BclExtensionMethods.Email;
	using Castle.Windsor;
	using Infrastructure;

	public class SetupEmail : IRunOnApplicationStart
	{
		public void Start(IWindsorContainer container)
		{
			// todo make a settings file entry for email debug mode
			var emailDebugMode = true; // Settings.Default.EmailDebugMode;
			if (emailDebugMode)
			{
				EmailConfiguration.Configuration.SetDebugMode();
			}
			else
			{
				EmailConfiguration.Configuration.SetSendMode();
			}
		}
	}
}