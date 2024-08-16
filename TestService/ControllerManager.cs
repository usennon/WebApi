using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService
{
    public static class ControllerManager
    {
        public static T CreateController<T>(Mock<IServiceManager>? mockServiceManager = null) where T : ControllerBase
        {
            var serviceManager = mockServiceManager ?? new Mock<IServiceManager>();

            var controller = Activator.CreateInstance(typeof(T), serviceManager.Object) as T;

            if (controller == null)
            {
                throw new InvalidOperationException($"Unable to create an instance of type {typeof(T)}.");
            }

            return controller;
        }
    }
}
