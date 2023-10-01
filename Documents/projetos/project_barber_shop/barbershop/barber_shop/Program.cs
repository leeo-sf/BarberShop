using barber_shop.Commands;
using barber_shop.Data;
using barber_shop.Integration;
using barber_shop.Integration.Interfces;
using barber_shop.Integration.Refit;
using barber_shop.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Refit;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var serverVersion = new MySqlServerVersion(new Version(8, 0, 30));

builder.Services.AddDbContext<Context>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("db_barber_shop"), serverVersion));

//classes de conexão com o banco
builder.Services.AddScoped<IBarberShopRepository, BarberShopRepository>();
builder.Services.AddScoped<IInsertClient, InsertClient>();
builder.Services.AddScoped<IInsertService, InsertService>();
builder.Services.AddScoped<IUpdateService, UpdateService>();
builder.Services.AddScoped<IDeleteService, DeleteService>();
builder.Services.AddScoped<IInsertScheduling, InsertScheduling>();
builder.Services.AddScoped<IGenerateReport, GenerateReport>();
builder.Services.AddScoped<BarberShopRepository>();
builder.Services.AddScoped<IViaCepIntegration, ViaCepIntegration>();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Access/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddRefitClient<IViaCepIntegrationRefit>().ConfigureHttpClient(x =>
    x.BaseAddress = new Uri("https://viacep.com.br")
    );

var app = builder.Build();

var enUS = new CultureInfo("en-US");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(enUS),
    SupportedCultures = new List<CultureInfo> { enUS },
    SupportedUICultures = new List<CultureInfo> { enUS }
};

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
