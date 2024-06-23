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

    var builder = WebApplication.CreateBuilder(args);

    // Configuring serilog service
    builder.Services.AddSerilog((services, config) => config
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

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

    builder.Services.AddInfrastructureServices(configuration);
    builder.Services.AddApplicationServices();

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
    Log.CloseAndFlush();
}
return 0;