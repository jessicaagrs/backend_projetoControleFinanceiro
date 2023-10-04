using APIControleFinanceiro.Models.CategoriasDespesas;
using APIControleFinanceiro.Models.CategoriasReceitas;
using APIControleFinanceiro.Models.Database;
using APIControleFinanceiro.Models.Despesas;
using APIControleFinanceiro.Models.Logins;
using APIControleFinanceiro.Models.Receitas;
using APIControleFinanceiro.Models.Usuarios;
using APIControleFinanceiro.Repositorios.CategoriasDespesas;
using APIControleFinanceiro.Repositorios.CategoriasReceitas;
using APIControleFinanceiro.Repositorios.Database;
using APIControleFinanceiro.Repositorios.Despesas;
using APIControleFinanceiro.Repositorios.Receitas;
using APIControleFinanceiro.Repositorios.Usuarios;
using APIControleFinanceiro.Servicos.Logins;
using APIControleFinanceiro.Servicos.Usuarios;
using APIControleFinanceiro.Validacoes.Usuarios;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

//servicos e repositorios
builder.Services.AddScoped<MongoDBContext>();
builder.Services.AddScoped<IUsuarioServico, UsuarioServico>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

builder.Services.AddScoped<ICategoriaReceitaRepositorio, CategoriaReceitaRepositorio>();
builder.Services.AddScoped<ICategoriaReceitaServico, CategoriaReceitaServico>();

builder.Services.AddScoped<ICategoriaDespesaRepositorio, CategoriaDespesaRepositorio>();
builder.Services.AddScoped<ICategoriaDespesaServico, CategoriaDespesaServico>();

builder.Services.AddScoped<IReceitaRepositorio, ReceitaRepositorio>();
builder.Services.AddScoped<IReceitaServico, ReceitaServico>();

builder.Services.AddScoped<IDespesaRepositorio, DespesaRepositorio>();
builder.Services.AddScoped<IDespesaServico, DespesaServico>();

builder.Services.AddScoped<ILoginServico, LoginServico>();

builder.Services.AddValidatorsFromAssemblyContaining<UsuarioValidacao>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//autenticacao
var key = builder.Configuration.GetSection("Key:MyKey").Value;

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

//swagger token
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Autorização",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Após gerar token, no campo Value abaixo digite Bearer [Seu token aqui]",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
