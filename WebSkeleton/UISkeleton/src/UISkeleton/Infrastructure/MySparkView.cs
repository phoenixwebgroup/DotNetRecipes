namespace UISkeleton.Infrastructure
{
	using System.Web.Mvc;
	using Spark.Web.Mvc;

	public abstract class MySparkView : SparkView
	{
	}

	public abstract class MySparkView<TModel> : SparkView<TModel>
		where TModel : class
	{
		public new HtmlHelper<TModel> Html
		{
			get
			{
				var result = base.Html as HtmlHelper<TModel>;
				if (result == null && base.Html != null)
				{
					result = new HtmlHelper<TModel>(base.Html.ViewContext,
					                                base.Html.ViewDataContainer);
					base.Html = result;
				}
				return result;
			}
		}
	}
}