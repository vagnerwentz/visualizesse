using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Visualizesse.API;
using Visualizesse.API.Endpoints;
using Visualizesse.Domain.Repositories;
using Visualizesse.Domain.Services;
using Visualizesse.Infrastructure;
using Visualizesse.Infrastructure.Auth;
using Visualizesse.Infrastructure.Repositories;
using Visualizesse.Service.Commands.User;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMediatR(
    cfg => cfg
                                    .RegisterServicesFromAssembly(typeof(SignUpCommand).Assembly));

builder.Services.AddHttpClient();
builder.Services.AddDbContext<DatabaseContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Visualizesse")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISubcategoryRepository, SubcategoryRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<TransactionService>();
builder.Services.AddTransient<SubcategoryService>();
builder.Services.AddTransient<WalletService>();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    swagger =>
    {
        swagger.OperationFilter<CustomHeaderSwaggerAttribute>();
        swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "Poupa Gasto Server", Version = "v1" });
        
        swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization."
        });
        
        swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                Array.Empty<string>()
            }
        });
    }
);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAnyOrigin");

app.RegisterUserEndpoints();
app.RegisterCategoriesEndpoints();
app.RegisterSubcategoriesEndpoints();
app.RegisterWalletEndpoints();
app.RegisterTransactionEndpoints();

app.Run();

public partial class Program { }