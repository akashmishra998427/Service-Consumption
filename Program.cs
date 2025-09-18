//using FluentValidation;
//using FluentValidation.AspNetCore;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//// Register FluentValidation
//builder.Services.AddControllers();
//builder.Services.AddValidatorsFromAssemblyContaining<Program>();
//// This scans the assembly and registers all validators

//builder.Services.AddFluentValidationAutoValidation(); // Enables auto-validation
//builder.Services.AddFluentValidationClientsideAdapters();

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using FluentValidation;
using FluentValidation.AspNetCore;
using ServiceConsumption.Models.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//// Register validators in DI
//builder.Services.AddValidatorsFromAssemblyContaining<Program>();

//// Enable FluentValidation automatic model validation
//builder.Services.AddFluentValidationAutoValidation();
//builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        // Register validators automatically from your assembly
        fv.RegisterValidatorsFromAssemblyContaining<PostApiEntityValidations>();
    });

//builder.Services.AddScoped<PostApiEntityValidations>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

