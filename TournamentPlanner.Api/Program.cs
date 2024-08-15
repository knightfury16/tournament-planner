using TournamentPlanner.Infrastructure;
using TournamentPlanner.Application;
using TournamentPlanner.Api.Middleware;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();


try
{
    Log.Information("Starting Up...");

    var builder = WebApplication.CreateBuilder(args);

    // Configuring serilog service
    //builder.Services.AddSerilog((services, config) => config
    //    .ReadFrom.Configuration(builder.Configuration)
    //    .ReadFrom.Services(services)
    //    .Enrich.FromLogContext());
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration)
                     .ReadFrom.Services(services)
                     .Enrich.FromLogContext()
                     .WriteTo.Console();
    });



    // Add services to the container.
    // Add CORS policy to allow all
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
    });

    builder.Services.AddControllers();
    var configuration = builder.Configuration;
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    try
    {

    builder.Services.AddInfrastructureServices(configuration);
    builder.Services.AddApplicationServices();
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while configuring services.");
        throw;
    }

    var app = builder.Build();

    // Use the CORS policy globally
    app.UseCors("AllowAll");

    //serilog config
    app.UseSerilogRequestLogging(option =>
    {
        option.MessageTemplate = "HTTP {RequestMethod} {RequestPath} ({UserId}) responded {StatusCode} in {Elapsed:0.0000}ms";
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.Run();
}
catch (System.Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;

}
finally
{
    Log.Information("Shutting down.");
    Log.CloseAndFlush();
}
return 0;