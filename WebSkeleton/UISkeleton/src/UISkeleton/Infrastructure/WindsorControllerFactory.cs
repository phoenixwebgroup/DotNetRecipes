namespace UISkeleton.Infrastructure
{
	using System;
	using System.Net;
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Mvc.Async;
	using System.Web.Routing;
	using Castle.Windsor;

	public class WindsorControllerFactory : DefaultControllerFactory
	{
		private readonly IWindsorContainer _Container;

		public WindsorControllerFactory(IWindsorContainer container)
		{
			_Container = container;
		}

		protected override IController GetControllerInstance(RequestContext context, Type controllerType)
		{
			if (controllerType != null)
			{
				var controller = _Container.Resolve(controllerType) as IController;
				if (controller != null)
				{
					if (controller is AsyncController)
					{
						(controller as AsyncController).ActionInvoker = _Container.Resolve<AsyncControllerActionInvoker>();
						return controller;
					}
					if (controller is Controller && _Container.Kernel.HasComponent(typeof (IActionInvoker)))
					{
						(controller as Controller).ActionInvoker = _Container.Resolve<IActionInvoker>();
					}
					return controller;
				}
			}

			throw new HttpException((int) HttpStatusCode.NotFound, "Invalid controller");
		}

		public override void ReleaseController(IController controller)
		{
			base.ReleaseController(controller);

			if (controller != null)
			{
				_Container.Kernel.ReleaseComponent(controller);
			}
		}
	}
}