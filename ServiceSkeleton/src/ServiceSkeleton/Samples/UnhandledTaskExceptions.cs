namespace ServiceSkeleton.Samples
{
	using System.Threading.Tasks;
	using Castle.Windsor;
	using Infrastructure;
	using log4net;

	public class UnhandledTaskExceptions : IConfigureOnStartup
	{
		public void Configure(IWindsorContainer container)
		{
			// this will handle unobserved task exceptions instead of killing the application
			TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
		}

		private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs exception)
		{
			exception.SetObserved();
			// todo pluging other unhandled exception handler
			LogManager.GetLogger(typeof (EndpointConfig)).Fatal("Unobserved exception", exception.Exception);
		}
	}
}