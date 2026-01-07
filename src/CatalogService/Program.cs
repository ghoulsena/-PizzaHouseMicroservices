var builder = WebApplication.CreateBuilder(args);



builder.Services
    .AddMyDependencyGroup()
    .AddMySwagger().
    AddMyDbContext(builder.Configuration);
var app = builder.Build();


 app.UseSwagger();
 app.UseSwaggerUI();

 app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();
app.Run();
