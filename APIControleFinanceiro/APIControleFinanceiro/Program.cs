using APIControleFinanceiro.Application.Servicos.Autenticacoes;
using APIControleFinanceiro.Application.Servicos.Autorizacoes;
using APIControleFinanceiro.Application.Servicos.CategoriasDespesas;
using APIControleFinanceiro.Application.Servicos.CategoriasReceitas;
using APIControleFinanceiro.Application.Servicos.Despesas;
using APIControleFinanceiro.Application.Servicos.Logins;
using APIControleFinanceiro.Application.Servicos.Receitas;
using APIControleFinanceiro.Application.Servicos.Usuarios;
using APIControleFinanceiro.Domain.Models.Autenticacoes;
using APIControleFinanceiro.Domain.Models.CategoriasDespesas;
using APIControleFinanceiro.Domain.Models.CategoriasReceitas;
using APIControleFinanceiro.Domain.Models.Database;
using APIControleFinanceiro.Domain.Models.Despesas;
using APIControleFinanceiro.Domain.Models.Logins;
using APIControleFinanceiro.Domain.Models.Receitas;
using APIControleFinanceiro.Domain.Models.Usuarios;
using APIControleFinanceiro.Domain.Validacoes.Usuarios;
using APIControleFinanceiro.Infra.Repositorios.Autenticacoes;
using APIControleFinanceiro.Infra.Repositorios.CategoriasDespesas;
using APIControleFinanceiro.Infra.Repositorios.CategoriasReceitas;
using APIControleFinanceiro.Infra.Repositorios.Database;
using APIControleFinanceiro.Infra.Repositorios.Despesas;
using APIControleFinanceiro.Infra.Repositorios.Receitas;
using APIControleFinanceiro.Infra.Repositorios.Usuarios;
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
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Documentação APIControleFinanceiro",
        Description = "Rotas da aplicação",
        Contact = new OpenApiContact() { Name = "Jessica", Email = "jessicaag.rs@gmail.com" },
        License = new OpenApiLicense()
        {
            Name = "JES Licence",
            Url = new Uri("https://github.com/jessicaagrs/backend_projetoControleFinanceiro")
        }
    });

    c.IncludeXmlComments(Path.Combine(System.AppContext.BaseDirectory, "DemoSwaggerAnnotation.xml"));

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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});


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

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(AutorizacaoFiltroServico));
});

var app = builder.Build();

var versionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
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

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
