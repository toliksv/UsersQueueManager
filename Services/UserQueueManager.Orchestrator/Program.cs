using Microsoft.AspNetCore;
using UserQueueManager.Orchestrator.Infrastructure;

await WebHost.CreateDefaultBuilder(args)
        .UseContentRoot(AppContext.BaseDirectory)
        .UseStartup<Startup>()
        .Build()
        .RunAsync();

