using APIControleFinanceiro.Models.Autenticacoes;
using APIControleFinanceiro.Models.CategoriasDespesas;
using APIControleFinanceiro.Models.CategoriasReceitas;
using APIControleFinanceiro.Models.Database;
using APIControleFinanceiro.Models.Despesas;
using APIControleFinanceiro.Models.Logins;
using APIControleFinanceiro.Models.Receitas;
using APIControleFinanceiro.Models.Usuarios;
using APIControleFinanceiro.Repositorios.Autenticacoes;
using APIControleFinanceiro.Repositorios.CategoriasDespesas;
using APIControleFinanceiro.Repositorios.CategoriasReceitas;
using APIControleFinanceiro.Repositorios.Database;
using APIControleFinanceiro.Repositorios.Despesas;
using APIControleFinanceiro.Repositorios.Receitas;
using APIControleFinanceiro.Repositorios.Usuarios;
using APIControleFinanceiro.Servicos.Autenticacoes;
using APIControleFinanceiro.Servicos.Autorizacoes;
using APIControleFinanceiro.Servicos.Logins;
using APIControleFinanceiro.Servicos.Receitas;
using APIControleFinanceiro.Servicos.Swagger;
using APIControleFinanceiro.Servicos.Usuarios;
using APIControleFinanceiro.Validacoes.Usuarios;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

//servicos e repositorios

builder.Services.AddTransient<MongoDBContext>();
builder.Services.AddTransient<IUsuarioServico, UsuarioServico>();
builder.Services.AddTransient<IUsuarioRepositorio, UsuarioRepositorio>();

builder.Services.AddTransient<ICategoriaReceitaRepositorio, CategoriaReceitaRepositorio>();
builder.Services.AddTransient<ICategoriaReceitaServico, CategoriaReceitaServico>();

builder.Services.AddTransient<ICategoriaDespesaRepositorio, CategoriaDespesaRepositorio>();
builder.Services.AddTransient<ICategoriaDespesaServico, CategoriaDespesaServico>();

builder.Services.AddTransient<IReceitaRepositorio, ReceitaRepositorio>();
builder.Services.AddTransient<IReceitaServico, ReceitaServico>();

builder.Services.AddTransient<IDespesaRepositorio, DespesaRepositorio>();
builder.Services.AddTransient<IDespesaServico, DespesaServico>();

builder.Services.AddTransient<ILoginServico, LoginServico>();

builder.Services.AddTransient<IRevogarTokensRepositorio, RevogarTokensRepositorio>();
builder.Services.AddTransient<IRevogarTokensServico, RevogarTokensServico>();

builder.Services.AddValidatorsFromAssemblyContaining<UsuarioValidacao>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();

//versao
builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});


builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<SwaggerDefaultValues>();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        Description = "Insira o token JWT desta maneira: Bearer {seu token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
    {
        new OpenApiSecurityScheme
        {
        Reference = new OpenApiReference
            {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,

        },
        new List<string>()
        }
    });


});

//cors

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

//autenticacao

var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Key:MyKey").Value);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//autorizacao

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(AutorizacaoFiltroServico));
});

var app = builder.Build();

var versionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in versionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                $"Web APi - {description.GroupName.ToUpper()}");
        }
    });
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseCors("MyPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
