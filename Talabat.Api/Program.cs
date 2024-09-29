using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Stripe;
using System.Text;
using Talabat.Api.AutoMapper;
using Talabat.Api.Errors;
using Talabat.Api.Extenstions;
using Talabat.Api.Extentions;
using Talabat.Api.MiddleWare;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Data;
using Talabat.Repository.Data.GenericRepository.RepositoriesContract;
using Talabat.Repository.Data.GenericRepository.ServicesContract;
using Talabat.Repository.Data.Repositories;
using Talabat.Repository.Identity;
using Talabat.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerServices();
builder.Services.AddDbContext<StoreContext>(con =>
{
    con.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDbContext<AppIdentityDbContext>(con =>
{
    con.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});
builder.Services.AddSingleton(typeof(ICacheResponseService), typeof(ResponseCacheService));

builder.Services.AddSingleton<IConnectionMultiplexer>((opt)=>
{
    var connection = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(connection);
});
builder.Services.AddApplicationServices();
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{

}).AddEntityFrameworkStores<AppIdentityDbContext>();
builder.Services.AddTokenServicesExtenstions(builder.Configuration);

builder.Services.AddCors(Opt =>
{
    Opt.AddPolicy("MyCors", opt =>
    {
        opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

var app = builder.Build();
using var Scope = app.Services.CreateScope();
var Service = Scope.ServiceProvider;
var dbcontext = Service.GetRequiredService<StoreContext>();
var Identitycontext = Service.GetRequiredService<AppIdentityDbContext>();



var LoggerFactory = Service.GetRequiredService<ILoggerFactory>();

try
{
    await dbcontext.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(dbcontext);
    await Identitycontext.Database.MigrateAsync();
    var userseed = Service.GetRequiredService<UserManager<AppUser>>();
    await AppIdentityDbContextSeed.SeedUserasync(userseed);
}
catch (Exception ex)
{
    var logger = LoggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "an error has accured during apply migration");
}
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.useSwaggermiddleware();
}
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("MyCors");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
