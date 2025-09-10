using System.Text.Json.Serialization;
using BusinessLogicLayer;
using BusinessLogicLayer.MappingProfile;
using DataAccessLayer;
using ECommerceOrders.API.APIEndpoints;
using FluentValidation.AspNetCore;

using ProductsAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBusinessLogicLayer();
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AllowNullCollections = true;
    cfg.AllowNullDestinationValues = true;
}, typeof(OrderMapProfile).Assembly);

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(cors =>
{
    cors.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("*");
    });
});

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
app.MapOrderApiEndpoints();
app.Run();