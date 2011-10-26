namespace ServiceSkeleton.Infrastructure.Status
{
	using System;

	[Serializable]
	public abstract class AsyncProgress
	{
		public AsyncProgress()
		{
			CreatedAt = DateTime.Now;
		}

		public DateTime CreatedAt { get; set; }
		public abstract string Progress { get; }
	}
}