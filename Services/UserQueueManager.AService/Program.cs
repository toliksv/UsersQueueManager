using Microsoft.AspNetCore;
using UserQueueManager.AService.Infrastructure;



await WebHost.CreateDefaultBuilder(args)
        .UseContentRoot(AppContext.BaseDirectory)
        .UseStartup<Startup>()
        .Build()
        .RunAsync();
