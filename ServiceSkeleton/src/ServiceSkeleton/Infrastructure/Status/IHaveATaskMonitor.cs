namespace ServiceSkeleton.Infrastructure.Status
{
	public interface IHaveATaskMonitor
	{
		TaskMonitor TaskMonitor { get; }
	}
}