using GenerateTicket.Consumers;
using GenerateTicket.Models;
using GenerateTicket.Services;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register MassTransit 
builder.Services.AddMassTransit(cfg =>
{
    cfg.AddBus(provider => MessageBrokers.RabbitMQ.ConfigureBus(provider));
    cfg.AddConsumer<GenerateTicketConsumer>(c => {
        c.UseMessageRetry(r =>
        {
            r.Ignore<ArgumentNullException>();
            r.Interval(3, TimeSpan.FromMilliseconds(1000));
        });
    });
    cfg.AddConsumer<CancelSendingEmailConsumer>();
});

// Connection string
var connectionString = builder.Configuration.GetConnectionString("DbConnection");

// Register AppDbContext
builder.Services.AddDbContextPool<AppDbContext>(db => db.UseSqlServer(connectionString));

// Register TicketInfo service
builder.Services.AddScoped<ITicketInfoService, TicketInfoService>();

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
