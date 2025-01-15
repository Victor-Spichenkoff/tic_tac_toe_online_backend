using asp_rest_model.Data;
using asp_rest_model.Helpers;
using asp_rest_model.Interface.Repositoy;
using asp_rest_model.MIddlewares;
using asp_rest_model.Services;
using asp_rest_model.Services.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<SocketService>();//sockets como service
builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseSqlite("Data Source=Data/dev.db"));
builder.Services.AddScoped<IUserRepository, UserRepository>();


var app = builder.Build();

//socket
app.UseWebSockets();
app.UseMiddleware<SocketMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // seeding
    app.MigrateDatabase();
}

app.UseHttpsRedirection();
app.MapControllers();

//socket
// Iniciar o SocketService ao inicializar a aplicação
//já gerencia tudo
// var socketService = app.Services.GetRequiredService<SocketService>();
// socketService.Start(); // Método que inicializa o servidor

app.Run();

//dotnet run --launch-profile http
