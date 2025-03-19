using kurs;
using kurs.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using kurs.Components.Account;
using kurs.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options => {
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BooksDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
// Добавление сервисов для работы с сессиями
builder.Services.AddDistributedMemoryCache(); // Для хранения сессий в памяти
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Время ожидания сессии
});

// Регистрация IHttpContextAccessor для доступа к HttpContext
builder.Services.AddHttpContextAccessor();

// Добавление сервисов Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Определение строки подключения
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Регистрация TableDbContext
builder.Services.AddDbContext<BooksDbContext>(options =>
    options.UseSqlServer(connectionString));

// Регистрация фабрики контекста для TableDbContext
builder.Services.AddDbContextFactory<BooksDbContext>(
    opt => opt.UseSqlServer(connectionString), ServiceLifetime.Scoped);

builder.Services.AddQuickGridEntityFrameworkAdapter();

// Добавление HTTP-клиента
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Включение сессий
app.UseSession();

// Включение защиты от подделки запросов (антифоржери)
app.UseAntiforgery();

// Маршруты для RazorComponents
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
