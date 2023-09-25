using APIControleFinanceiro.Models.CategoriasReceitas;
using APIControleFinanceiro.Models.Usuarios;
using APIControleFinanceiro.Repositorios.CategoriasReceitas;
using APIControleFinanceiro.Repositorios.Database;
using APIControleFinanceiro.Repositorios.Usuarios;
using APIControleFinanceiro.Servicos.Usuarios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<UsuariosDatabaseSettings>(builder.Configuration.GetSection("UsuariosDatabaseSettings"));
builder.Services.Configure<CategoriasReceitasDatabaseSettings>(builder.Configuration.GetSection("CategoriasReceitasDatabaseSettings"));

builder.Services.AddSingleton<UsuarioRepositorio>();
builder.Services.AddScoped<IUsuarioServico, UsuarioServico>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

builder.Services.AddSingleton<CategoriaReceitaRepositorio>();
builder.Services.AddScoped<ICategoriaReceitaRepositorio, CategoriaReceitaRepositorio>();
builder.Services.AddScoped<ICategoriaReceitaServico, CategoriaReceitaServico>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
