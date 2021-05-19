using EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace EventBus
{
    public abstract class SubscribeProcessEvent
    {
        protected readonly ILogger<IEventBus> _logger;
        protected readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _appName;

        protected SubscribeProcessEvent(ILogger<IEventBus> logger,
            IEventBusSubscriptionsManager subsManager,
            IServiceProvider serviceProvider,
            string appName)
        {
            _logger = logger;
            _subsManager = subsManager;
            _serviceProvider = serviceProvider;
            _appName = appName;
        }

        protected virtual async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace($"Processing {_appName} event: {{EventName}}", eventName);

            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    IServiceProvider serviceProvider = scope.ServiceProvider;

                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);

                    foreach (var subscription in subscriptions)
                    {
                        if (subscription.IsDynamic)
                        {
                            var handler = serviceProvider.GetService(subscription.HandlerType) as IDynamicIntegrationEventHandler;

                            if (handler == null) continue;
                            dynamic eventData = JObject.Parse(message);

                            await handler.Handle(eventData);
                        }
                        else
                        {
                            var handler = serviceProvider.GetService(subscription.HandlerType);

                            if (handler == null) continue;

                            var eventType = _subsManager.GetEventTypeByName(eventName);
                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }
            }
            else
            {
                _logger.LogWarning($"No subscription for {_appName} event: {{EventName}}", eventName);
            }
        }

    }
}
