namespace UISkeleton.Samples
{
	using System.Web.Mvc;
	using Castle.Windsor;
	using Infrastructure;
	using Spark.Web.Mvc;

	public class ConfigureSpark : IConfigureOnStartup
	{
		public void Configure(IWindsorContainer container)
		{
			ViewEngines.Engines.Add(new SparkViewFactory());
		}
	}
}