using backend.super_chatbot;
using backend.super_chatbot.Apis;
using backend.super_chatbot.Configuration;
using backend.super_chatbot.Middleware;
using backend.super_chatbot.Repositories;
using backend.super_chatbot.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var configuration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environmentName}.json", true, true)
    .AddEnvironmentVariables().Build();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IWebhookService, WebhookService>();

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

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.Run();
