using CarDealershipManager.Infrastructure;
using CarDealershipManager.Infrastructure.Data;
using CarDealershipManager.Infrastructure.Identity;
using CarDealershipManager.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Banco de Dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
}).AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>();

// Configuração de caminhos padrão de autenticação/autorização
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.ConfigurationOptions = new ConfigurationOptions
    {
        EndPoints = { "localhost:6379" },
        ConnectTimeout = 1000,
        SyncTimeout = 1000,
        AsyncTimeout = 1000,
        ConnectRetry = 2,
        ReconnectRetryPolicy = new ExponentialRetry(1000),
        AbortOnConnectFail = true
    };
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure();
builder.Services.AddRazorPages();

// Swagger API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CarDealershipManager API", Version = "v1" });
});

var app = builder.Build();

// Cultura para pt-BR
var supportedCultures = new[] { new CultureInfo("pt-BR") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarDealershipManager API v1"));

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Create and seed database (if needed)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
    await SeedDatabase(context, userManager, roleManager);
}

app.Run();

static async Task SeedDatabase(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    await context.Database.MigrateAsync();

    // Create default roles
    var roles = new[] { "Administrador", "Gerente", "Vendedor" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Create default admin user
    var adminEmail = "admin@cardealership.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            NomeCompleto = "Administrador do Sistema",
            NivelAcesso = CarDealershipManager.Core.Enums.NivelAcesso.Administrador,
            EmailConfirmed = true,
            DataCriacao = DateTime.UtcNow,
            Ativo = true
        };

        var result = await userManager.CreateAsync(adminUser, "Admin@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Administrador");
        }
    }

    // Create default manager user
    var gerenteEmail = "gerente@cardealership.com";
    var gerenteUser = await userManager.FindByEmailAsync(gerenteEmail);

    if (gerenteUser == null)
    {
        gerenteUser = new ApplicationUser
        {
            UserName = gerenteEmail,
            Email = gerenteEmail,
            NomeCompleto = "Gerente da Concessionária",
            NivelAcesso = CarDealershipManager.Core.Enums.NivelAcesso.Gerente,
            EmailConfirmed = true,
            DataCriacao = DateTime.UtcNow,
            Ativo = true
        };

        var result = await userManager.CreateAsync(gerenteUser, "Gerente@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(gerenteUser, "Gerente");
        }
    }

    // Create default seller user
    var vendedorEmail = "vendedor@cardealership.com";
    var vendedorUser = await userManager.FindByEmailAsync(vendedorEmail);

    if (vendedorUser == null)
    {
        vendedorUser = new ApplicationUser
        {
            UserName = vendedorEmail,
            Email = vendedorEmail,
            NomeCompleto = "Vendedor da Concessionária",
            NivelAcesso = CarDealershipManager.Core.Enums.NivelAcesso.Vendedor,
            EmailConfirmed = true,
            DataCriacao = DateTime.UtcNow,
            Ativo = true
        };

        var result = await userManager.CreateAsync(vendedorUser, "Vendedor@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(vendedorUser, "Vendedor");
        }
    }
}