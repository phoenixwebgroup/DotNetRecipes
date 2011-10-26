namespace ServiceSkeleton.Infrastructure.Status
{
	using System;
	using log4net;

	[Serializable]
	public abstract class AsyncProgressWithPercentage : AsyncProgress
	{
		public AsyncProgressWithPercentage()
		{
			Stage = string.Empty;
		}

		private string _Stage;

		protected string Stage
		{
			get { return _Stage; }
			set
			{
				_Stage = value;
				GetLog().Debug(Progress);
			}
		}

		protected int _Count;

		public int Done;

		public override string Progress
		{
			get
			{
				return (_Count > 0 && Done > 0)
				       	? string.Format("{2:0.00}% done ({0}/{1})", Done, _Count, ((decimal) Done)/(_Count)*100)
				       	: _Stage;
			}
		}

		protected ILog GetLog()
		{
			return LogManager.GetLogger(typeof (AsyncProgressWithPercentage).FullName);
		}
	}
}