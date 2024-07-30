using Microsoft.EntityFrameworkCore;
using Quartz;
using WeatherApi.DataAccess;
using Quartz.Simpl;
using WeatherApi.Services;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder.Services);

var app = builder.Build();

Configure(app);

app.Run();

void RegisterServices(IServiceCollection services)
{
    services.AddControllers();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddQuartz(q =>
    {
        // Use a Scoped container to create the job class.
        q.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();

        // Register the job and trigger
        var jobKey = new JobKey("WeatherJob");
        q.AddJob<WeatherJob>(opts => opts.WithIdentity(jobKey));

        q.AddTrigger(opts => opts
            .ForJob(jobKey) 
            .WithIdentity("WeatherJob-trigger")
            .StartNow() 
            .WithSimpleSchedule(x => x
                .WithInterval(TimeSpan.FromHours(3)) 
                .RepeatForever())); 
    });

    // Add Quartz.NET hosted service
    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

    services.AddDbContext<WeatherDb>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(WeatherDb)));
    });

    services.AddTransient<WeatherService>();

    builder.Services.AddHttpClient<WeatherService>("Report", client =>
{
    client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
});
}

void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WeatherDb>();
        db.Database.EnsureCreated();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
}
