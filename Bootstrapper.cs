using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MVVM.Template.Config;
using MVVM.Template.View;
using MVVM.Template.ViewModel;
using Serilog;
using Serilog.Extensions.Logging;
using System.Net;
using System.Net.Http;

namespace MVVM.Template.Utilities;

public static class Bootstrapper
{
    private static IContainer? _container;

    public static void Start()
    {
        var builder = new ContainerBuilder()
            .Config()
            .ConfigureLogging()
            .Setup();

        _container = builder.Build();

    }

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
    public static T Resolve<T>() => _container!.Resolve<T>();
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

    private static ContainerBuilder Config(this ContainerBuilder builder)
    {
        var configBuilder = new ConfigurationBuilder();
        // configBuilder.SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json", false);
        configBuilder.AddJsonFile("Config/appsettings.json", false);
        var config = configBuilder.Build();

        builder.Register(x =>
        {
            var c = new AppSettings();
            config.GetSection(nameof(AppSettings)).Bind(c);
            return c;
        });
        builder.RegisterInstance(config).AsImplementedInterfaces();

        return builder;
    }

    public static ContainerBuilder ConfigureLogging(this ContainerBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.Console()
            .CreateLogger();

        builder.Register(_ => new LoggerFactory(new ILoggerProvider[] { new SerilogLoggerProvider() }))
            .As<ILoggerFactory>()
            .SingleInstance();
        builder.RegisterGeneric(typeof(Logger<>))
            .As(typeof(ILogger<>));

        return builder;
    }

    private static ContainerBuilder Setup(this ContainerBuilder builder)
    {
        builder.RegisterType<MainWindow>();
        builder.RegisterType<MainWindowViewModel>().SingleInstance();

        return builder;
    }

    public static void Stop() => _container?.Dispose();
}
