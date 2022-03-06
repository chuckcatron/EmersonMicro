using centralProcessing.DAL;
using centralProcessing.Helpers;
using centralProcessing.Interfaces;
using centralProcessing.MessageCenter;
using centralProcessing.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace centralProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            using ServiceProvider serviceProvider = services.BuildServiceProvider();
            CentralProcessing app = serviceProvider.GetService<CentralProcessing>();
            // Start up logic here
            app?.Run();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services
                .AddLogging(configure =>
                {
                    configure.AddConsole();
                    configure.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Critical);
                })
                .AddTransient<CentralProcessing>()
                .AddDbContext<centraldbContext>()
                .AddSingleton<IPubSocket, PubSocket>()
                .AddSingleton<IScreenHelper, ScreenHelper>()
                .AddSingleton<IFlowRouting, FlowRouting>()
                .AddSingleton<IChannelRepository, ChannelRepository>()
                .BuildServiceProvider();
        }
    }
}
