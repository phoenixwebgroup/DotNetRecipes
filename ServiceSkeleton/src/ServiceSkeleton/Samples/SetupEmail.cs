namespace ServiceSkeleton.Samples
{
	using BclExtensionMethods.Email;
	using Castle.Windsor;
	using Infrastructure;

	public class SetupEmail : IConfigureOnStartup
	{
		public void Configure(IWindsorContainer container)
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