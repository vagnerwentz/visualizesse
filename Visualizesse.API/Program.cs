using Microsoft.EntityFrameworkCore;
using Visualizesse.API;
using Visualizesse.API.Endpoints;
using Visualizesse.Domain.Event;
using Visualizesse.Domain.Repositories;
using Visualizesse.Infrastructure;
using Visualizesse.Infrastructure.Repositories;
using Visualizesse.Service.Commands.User;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMediatR(
    cfg => cfg
                                    .RegisterServicesFromAssembly(typeof(SignInCommand).Assembly));

builder.Services.AddHttpClient();
builder.Services.AddDbContext<DatabaseContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Visualizesse")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISubcategoryRepository, SubcategoryRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();

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
    }
);

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

app.RegisterUserEndpoints();
app.RegisterTransactionEndpoints();
app.RegisterCategoriesEndpoints();
app.RegisterSubcategoriesEndpoints();
app.RegisterWalletEndpoints();

app.Run();