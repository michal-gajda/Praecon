namespace Praecon.WinUI;

using System.Reflection;
using System.Windows;

using CommunityToolkit.Mvvm.DependencyInjection;

using Dapper;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MvvmDialogs;
using MvvmDialogs.DialogTypeLocators;

using Praecon.WinUI.Models.Interfaces;
using Praecon.WinUI.Models.Profiles;
using Praecon.WinUI.Models.Services;
using Praecon.WinUI.ViewModels;

public partial class App : Application
{
    protected override void OnStartup(System.Windows.StartupEventArgs e)
    {
        base.OnStartup(e);

        IServiceCollection services = new ServiceCollection();

        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        services.AddSingleton(configuration);

        services.AddLogging(cfg => cfg.AddSeq(configuration.GetSection("Seq")));

        Assembly? assembly = System.Reflection.Assembly.GetExecutingAssembly();
        services.AddAutoMapper(assembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        SqlMapper.AddTypeHandler(new NullableDateOnlyTypeHandler());

        SqlServerOptions? options = new SqlServerOptions
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnectionString")!,
        };

        services.AddSingleton(options);

        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<ShellViewModel>();
        services.AddSingleton<UpdateArticleViewModel>();
        services.AddSingleton<IArticleRepository, ArticleRepository>();
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<IDialogTypeLocator, DialogTypeLocator>();

        IServiceProvider provider = services.BuildServiceProvider();

        Ioc.Default.ConfigureServices(provider);
    }
}
