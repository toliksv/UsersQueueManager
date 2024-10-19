using Microsoft.AspNetCore;
using UserQueueManager.BService.Infrastructure;

await WebHost.CreateDefaultBuilder(args)
        .UseContentRoot(AppContext.BaseDirectory)
        .UseStartup<Startup>()
        .Build()
        .RunAsync();

