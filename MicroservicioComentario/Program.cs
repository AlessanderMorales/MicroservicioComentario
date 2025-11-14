using MicroservicioComentario.Application.Services;
using MicroservicioComentario.Domain.Interfaces;
using MicroservicioComentario.Domain.Entities;
using MicroservicioComentario.Infrastructure.Persistence;
using MicroservicioComentario.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<MySqlConnectionSingleton>();
builder.Services.AddScoped<IRepository<Comentario>, ComentarioRepository>();
builder.Services.AddScoped<ComentarioService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
