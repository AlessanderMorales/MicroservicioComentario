using MicroservicioComentario.Application.Services;
using MicroservicioComentario.Domain.Interfaces;
using MicroservicioComentario.Domain.Entities;
using MicroservicioComentario.Infrastructure.Repository;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ----------------------------------------------------
//   REGISTRAR MySqlConnection PARA ESTE MICROSERVICIO
// ----------------------------------------------------
builder.Services.AddScoped<MySqlConnection>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connString = config.GetConnectionString("ComentariosConnection");

    var conn = new MySqlConnection(connString);
    conn.Open();  // abrir una vez por request
    return conn;
});

// ----------------------------------------------------
//   REPOSITORIO Y SERVICIO
// ----------------------------------------------------
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
