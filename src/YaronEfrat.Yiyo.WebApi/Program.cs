using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

using YaronEfrat.Yiyo.Application;
using YaronEfrat.Yiyo.Persistence;
using YaronEfrat.Yiyo.WebApi.Controllers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddApplicationLayerDependencyInjection();
builder.Services.AddPersistenceLayerDependencyInjection(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<YearInController>());

builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddDebug());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
