using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Morgan.SalesforceSyncPOC.Core.Contracts;
using Morgan.SalesforceSyncPOC.Core.DataModels;
using Morgan.SalesforceSyncPOC.Core.Events;
using Morgan.SalesforceSyncPOC.Infrastrcture.Data;
using Morgan.SalesforceSyncPOC.Infrastrcture.Repositories;
using Morgan.SalesforceSyncPOC.Integration.Outbox;
using Morgan.SalesforceSyncPOC.Integration.ServiceBus;
using Morgan.SalesforceSyncPOC.WebApi.Logging;
using Morgan.SalesforceSyncPOC.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["KeyVaultUrl"] ?? "https://kv-morgan-poc-dev.vault.azure.net/"), new DefaultAzureCredential());

string SqlConnString = builder.Configuration["SqlConnectionString"];
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(SqlConnString)
);
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<OutboxMessage>, OutboxMessageRepository>();


//Event integration
builder.Services.AddServiceBus(builder.Configuration);
builder.Services.AddOutbox(builder.Configuration);

#region Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
        else
        {
            var allowedOrigins = builder.Configuration
                .GetSection("Cors:AllowedOrigins")
                .Get<string[]>() ?? [];

            policy
                .WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});
#endregion
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.ConfigureSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
