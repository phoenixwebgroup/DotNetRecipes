namespace ServiceSkeleton.Infrastructure.Status
{
	using System.Collections.Generic;

	public interface IHaveStatusToReport
	{
		IEnumerable<object> MyStatuses();
	}
}