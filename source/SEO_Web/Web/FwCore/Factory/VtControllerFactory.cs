using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Custom;

namespace Web.FwCore.Factory
{
    public class VtControllerFactory : DefaultControllerFactory
    {
        private IKernel kernel;

        public VtControllerFactory()
        {
            kernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            var obj = controllerType == null
            ? null
            : (IController)kernel.Get(controllerType);

            if (obj == null)
            {
                
                //throw new HttpException(404, "Not Found");
            }

            return obj;
        }

        private void AddBindings()
        {
            DIBinding.AddBinding(kernel);
        }
    }
}