using APIControleFinanceiro.Models.CategoriasDespesas;
using APIControleFinanceiro.Models.CategoriasReceitas;
using APIControleFinanceiro.Models.Receitas;
using APIControleFinanceiro.Models.Usuarios;
using APIControleFinanceiro.Repositorios.CategoriasDespesas;
using APIControleFinanceiro.Repositorios.CategoriasReceitas;
using APIControleFinanceiro.Repositorios.Database;
using APIControleFinanceiro.Repositorios.Receitas;
using APIControleFinanceiro.Repositorios.Usuarios;
using APIControleFinanceiro.Servicos.Usuarios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<UsuariosDatabaseSettings>(builder.Configuration.GetSection("UsuariosDatabaseSettings"));
builder.Services.Configure<CategoriasReceitasDatabaseSettings>(builder.Configuration.GetSection("CategoriasReceitasDatabaseSettings"));
builder.Services.Configure<CategoriasDespesasDatabaseSettings>(builder.Configuration.GetSection("CategoriasDespesasDatabaseSettings"));
builder.Services.Configure<ReceitaDatabaseSettings>(builder.Configuration.GetSection("ReceitasDatabaseSettings"));

builder.Services.AddScoped<IUsuarioServico, UsuarioServico>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

builder.Services.AddScoped<ICategoriaReceitaRepositorio, CategoriaReceitaRepositorio>();
builder.Services.AddScoped<ICategoriaReceitaServico, CategoriaReceitaServico>();

builder.Services.AddScoped<ICategoriaDespesaRepositorio, CategoriaDespesaRepositorio>();
builder.Services.AddScoped<ICategoriaDespesaServico, CategoriaDespesaServico>();

builder.Services.AddScoped<IReceitaRepositorio, ReceitaRepositorio>();
builder.Services.AddScoped<IReceitaServico, ReceitaServico>();

builder.Services.AddControllers();
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
