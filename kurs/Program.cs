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
// ���������� �������� ��� ������ � ��������
builder.Services.AddDistributedMemoryCache(); // ��� �������� ������ � ������
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // ����� �������� ������
});

// ����������� IHttpContextAccessor ��� ������� � HttpContext
builder.Services.AddHttpContextAccessor();

// ���������� �������� Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ����������� ������ �����������
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// ����������� TableDbContext
builder.Services.AddDbContext<BooksDbContext>(options =>
    options.UseSqlServer(connectionString));

// ����������� ������� ��������� ��� TableDbContext
builder.Services.AddDbContextFactory<BooksDbContext>(
    opt => opt.UseSqlServer(connectionString), ServiceLifetime.Scoped);

builder.Services.AddQuickGridEntityFrameworkAdapter();

// ���������� HTTP-�������
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

// ��������� ������
app.UseSession();

// ��������� ������ �� �������� �������� (�����������)
app.UseAntiforgery();

// �������� ��� RazorComponents
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
