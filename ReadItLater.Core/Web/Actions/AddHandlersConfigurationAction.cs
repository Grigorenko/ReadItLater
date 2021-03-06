using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Web;
using ReadItLater.Core.Infrastructure.Attributes;

namespace ReadItLater.Core.Api.Actions
{
    public static class AddHandlersConfigurationAction
    {
        public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
        {
            Action<Assembly> action = assembly =>
            {
                var commandHandlers = assembly.GetTypes()
                    .Where(FilterTypesBy(typeof(IAsyncQueryHandler<,>)))
                    .ToList();

                commandHandlers.ForEach(t => AddQueryHandler(services, t));
                Console.WriteLine("Query handlers registered: \n{0}", string.Join("\n", commandHandlers.Select(h => h.FullName)));
            };

            return AddHandlers(services, action);
        }

        public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            Action<Assembly> action = assembly =>
            {
                var commandHandlers = assembly.GetTypes()
                    .Where(FilterTypesBy(typeof(IAsyncCommandHandler<>)))
                    .ToList();

                commandHandlers.ForEach(t => AddCommandHandler(services, t));
                Console.WriteLine("Command handlers registered: \n{0}", string.Join("\n", commandHandlers.Select(h => h.FullName)));
            };

            return AddHandlers(services, action);
        }

        private static IServiceCollection AddHandlers(IServiceCollection services, Action<Assembly> action)
        {
            var configuration = services.GetConfiguration();
            var assemblyName = configuration
                .GetSection(AssembliesConfiguration.Section)
                .GetValue<string?>(nameof(AssembliesConfiguration.Infrastructure));

            if (string.IsNullOrEmpty(assemblyName))
                throw new ArgumentNullException(nameof(AssembliesConfiguration.Section));

            var assembly = Assembly.Load(new AssemblyName(assemblyName));

            action(assembly);

            return services;
        }

        private static Func<Type, bool> FilterTypesBy(Type interfaceType) =>
            x => x.GetInterfaces().Any(t => IsHandlerInterface(t, interfaceType)) && x.Name.EndsWith("Handler");

        private static void AddCommandHandler(IServiceCollection services, Type type) =>
            CreatePipeline<ICommandHandlerDecoratorAttribute>(services, type);

        private static void AddQueryHandler(IServiceCollection services, Type type) =>
            CreatePipeline<IQueryHandlerDecoratorAttribute>(services, type);

        private static void CreatePipeline<TAttribute>(IServiceCollection services, Type type)
        {
            var attributes = type
                .GetCustomAttributesData()
                .Where(a => a.AttributeType.GetInterfaces().Any(i => i == typeof(TAttribute)));

            var pipeline = attributes
                            .Select(ToDecorator)
                            .Concat(new[] { type })
                            .Reverse()
                            .ToList();

            var interfaceType = type.GetInterfaces().Single(IsHandlerInterface);
            var factory = BuildPipeline(pipeline, interfaceType);

            if (!(factory is null))
                services.AddTransient(interfaceType, factory!);
        }

        private static Func<IServiceProvider, object?> BuildPipeline(IEnumerable<Type> pipeline, Type interfaceType)
        {
            var ctors = pipeline
                .Select(x =>
                {
                    var type = x.IsGenericType ? x.MakeGenericType(interfaceType.GenericTypeArguments) : x;
                    return type.GetConstructors().Single();
                })
                .ToList();

            Func<IServiceProvider, object?> func = provider =>
            {
                object? current = null;

                ctors.ForEach(c => current = c.Invoke(GetParameters(c.GetParameters(), current, provider)));

                return current;
            };

            return func;
        }

        private static object?[] GetParameters(IReadOnlyList<ParameterInfo> parameterInfos, object? current, IServiceProvider provider) =>
            parameterInfos
                .Select(i =>
                {
                    var parameterType = i.ParameterType;

                    if (IsHandlerInterface(parameterType))
                        return current;

                    var service = provider.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope()
                        .ServiceProvider
                        .GetService(parameterType);

                    if (service != null)
                        return service;

                    throw new ArgumentException($"Type {parameterType} not found");
                })
                .ToArray();

        private static Type ToDecorator(CustomAttributeData? attribute)
        {
            if (!attribute.HasValue())
                throw new ArgumentNullException(nameof(CustomAttributeData));

            if (Activator.CreateInstance(attribute!.AttributeType) is IHandlerDecoratorAttribute attr)
                return attr.DecoratorType;

            throw new ArgumentException(attribute.ToString());
        }

        private static bool IsHandlerInterface(Type type) =>
            IsHandlerInterface(type, typeof(IAsyncCommandHandler<>)) || IsHandlerInterface(type, typeof(IAsyncQueryHandler<,>));

        private static bool IsHandlerInterface(Type type, Type applicableType)
        {
            if (!type.IsGenericType)
                return false;

            var typeDefinition = type.GetGenericTypeDefinition();

            return typeDefinition == applicableType;
        }
    }
}
