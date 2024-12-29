using AutoReels.Studio.Identity.API;
using AutoReels.Studio.Identity.Business;
using AutoReels.Studio.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDataServices(builder.Configuration);
builder.Services.AddBusinessServices();
builder.Services.AddApiServices();

var app = builder.Build();
app.UseHttpLogging();
app.UseResponseCompression();
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();

app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();
