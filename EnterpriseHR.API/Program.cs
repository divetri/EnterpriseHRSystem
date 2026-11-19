using EnterpriseHR.Api.Middleware;
using EnterpriseHR.DataAccess;
using EnterpriseHR.DataAccess.Repositories;
using EnterpriseHR.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrasi DataAccess & Repositories
builder.Services.AddSingleton<SqlConnectionFactory>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
//builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IMasterDataRepository, MasterDataRepository>();

var app = builder.Build();

// Global Exception Handling Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();