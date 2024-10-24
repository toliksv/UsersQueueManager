using System;
using System.Reflection;
using UserQueueManager.Core.Infrastructure;
using UserQueueManager.FileRepository.Infrastructure;
using UserQueueManager.Orchestrator.Core.RequestsProcessing;

namespace UserQueueManager.Orchestrator.Infrastructure;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"), true);
        });

        services.AddLogging(builder => builder.AddConsole());
        services.AddMontorServicesControllers(Configuration);
        services.AddCoreHandlers();
        services.AddProductQueueStorage();
        services.UseFileRepository(Configuration);
        services.RegisterStartConfiguration(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        if (env.IsDevelopment())
        {
            // In Development, use the Developer Exception Page
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            // In Staging/Production, route exceptions to /error
            app.UseExceptionHandler("/error");
        }

        app.UseRouting();
        app.UseEndpoints(enp => enp.MapControllers());
    }
}
