using System;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Pooyan.test.infrastructure.StatesBase;
using Pooyan.test.project.StateManagement.StateManagerExtensions;
using Pooyan.test.project.StateManagement.States;

namespace Pooyan.test.project.StateManagement
{
    public class StateContext
    {
        public StateBase<UserState> UserState { get; set; }
        public StateBase<BusinessState> BusinessState { get; set; }
        public StateBase<CustomerState> CustomerState { get; set; }

        public static void ConfigurationNullRefrenceExceptions(IServiceProvider serviceProvider)
        {
            StateBaseExtensions.ConfigLocalStorage(serviceProvider);
        }

        public static void ConfigurationFreeezeProblem(ILocalStorageService localStorage)
        {
            StateBaseExtensions.ConfigLocalStorage(localStorage);
        }

        public StateContext()
        {
            if (!StateBaseExtensions.HandleProblems)
            {
                var props = this.GetType().GetProperties().ToList().Where(a => a.PropertyType.GetGenericTypeDefinition() == typeof(StateBase<>));
                foreach (var item in props)
                {
                    var value = item.GetValue(this) ?? Activator.CreateInstance(item.PropertyType);
                    ((Task)typeof(StateBaseExtensions).GetMethod("GetItemAsync").MakeGenericMethod(item.PropertyType.GetGenericArguments()[0]).Invoke(null, new object[] { value })).Wait();
                }
            }
        }
        public async Task Initialize(IServiceProvider ServiceProvider)
        {
            var context = ServiceProvider.GetRequiredService<StateContext>();
            var props = context.GetType().GetProperties().ToList().Where(a => a.PropertyType.GetGenericTypeDefinition() == typeof(StateBase<>));
            foreach (var item in props)
            {
                var value = item.GetValue(context) ?? Activator.CreateInstance(item.PropertyType);
                await ((Task)typeof(StateBaseExtensions).GetMethod("GetItemAsync").MakeGenericMethod(item.PropertyType.GetGenericArguments()[0]).Invoke(null, new object[] { value }));
            }
        }
    }
}
