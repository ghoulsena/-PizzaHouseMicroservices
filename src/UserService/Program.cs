var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMyDependencyGrop()
    .AddMySwagger()
    .AddMyAddAuthentication(builder.Configuration)
    .AddMyDbContext(builder.Configuration);



var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
