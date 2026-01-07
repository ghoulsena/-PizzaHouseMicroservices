using DeliveryService.Extensions;
using DeliveryService.Http;
using DeliveryService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
//builder.WebHost.UseUrls("http://0.0.0.0:80");
builder.Services.AddMyDependencyGroup();
builder.Services.AddHttpClient<DeliveryServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5012"); 
});




builder.Services.AddDbContext<CourierDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();

app.Run();
