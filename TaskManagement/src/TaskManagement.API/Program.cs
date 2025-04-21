using FluentValidation.AspNetCore;
using System.Text.Json.Serialization;
using TaskManagement.API.Middleware;
using TaskManagement.Application;
using TaskManagement.Domain;
using TaskManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

// Add Dependency Injection
builder.Services.AddApplicationDependencyInjection();
builder.Services.AddDomainDependencyInjection();
builder.Services.AddInfrastructureDependencyInjection(builder.Configuration);

//FluentValidations
builder.Services.AddFluentValidationAutoValidation();

//Add model binder to read values from JSON to enum
builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

//Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cors
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(builder => {
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});


var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();

//Cors
app.UseCors();

//Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();