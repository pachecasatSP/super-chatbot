using backend.super_chatbot;
using backend.super_chatbot.Apis;
using backend.super_chatbot.Configuration;
using backend.super_chatbot.Middleware;
using backend.super_chatbot.Repositories;
using backend.super_chatbot.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    ContractResolver = new CamelCasePropertyNamesContractResolver(),
    Formatting = Formatting.Indented,
    NullValueHandling = NullValueHandling.Ignore,
    DateFormatString = "dd-MM-yyyy",
    DefaultValueHandling = DefaultValueHandling.Ignore,
    MaxDepth = 3
};

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var configuration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environmentName}.json", true, true)
    .AddEnvironmentVariables().Build();


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database", LogEventLevel.Error)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(new ElasticsearchJsonFormatter())
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration.GetSection("ElasticSearch__Uri").Value!))
    {
        ApiKey = configuration.GetSection("ElasticSearch__ApiKey").Value!,
        AutoRegisterTemplate = true,
        IndexFormat = "chat-webhook-{0:yyyy.MM.dd}",
        FailureCallback = (l, e) => Console.WriteLine("Falha ao enviar evento para o Elasticsearch: " + e.Message),
        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog
    })
    .CreateLogger();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IMetaService, MetaService>();

builder.Services.Configure<WABConfiguration>(configuration.GetSection(WABConfiguration.WABOptions));

builder.Services.AddDbContext<SuperChatContext>((DbContextOptionsBuilder options) =>
{
    options.UseSqlServer(connectionString: configuration.GetConnectionString("Default") ?? string.Empty, dboptions =>
    {
        dboptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(1), null);
        dboptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        dboptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
    });
});

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapClientEndpoints();
app.MapWebhookEndpoints();
app.MapSendMessageEndpoints();

app.UseSerilogRequestLogging();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.Run();
