
using Microsoft.AspNetCore.Authorization;
using OrderService.Presentation.OrderOwnerRequirement;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAuthorizationHandler, OrderOwnHandler>();


builder.Services
    .AddMyDependencyGroup()
    .AddMySwagger(builder.Configuration)
    .AddDbContext(builder.Configuration)
    .AddMyAddAuthentication(builder.Configuration);



var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
