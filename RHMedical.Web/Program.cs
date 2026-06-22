using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MudBlazor.Services;
using RHMedical.Application;
using RHMedical.Application.Users;
using RHMedical.Data.Persistence;
using RHMedical.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMudServices();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
        options.Events ??= new OpenIdConnectEvents();

        options.Events.OnTokenValidated = async context =>
        {
            var userProvisioningService =
                context.HttpContext.RequestServices
                    .GetRequiredService<IUserProvisioningService>();

            var claims = context.Principal!.Claims;

            var azureObjectId =
                claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
                ?? claims.FirstOrDefault(x => x.Type == "oid")?.Value;

            var email =
                claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value
                ?? claims.FirstOrDefault(x => x.Type == "email")?.Value
                ?? context.Principal.Identity?.Name;

            var fullName =
                claims.FirstOrDefault(x => x.Type == "name")?.Value
                ?? email
                ?? "Unknown User";

            await userProvisioningService.ProvisionUserAsync(
                azureObjectId!,
                email!,
                fullName);
        };
    });
builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();
builder.Services.AddAuthorization();

var app = builder.Build();

//automatic run migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapGet("/logout", async context =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    await context.SignOutAsync(
        OpenIdConnectDefaults.AuthenticationScheme,
        new AuthenticationProperties
        {
            RedirectUri = builder.Configuration["Application:LoginUrl"]
        });
});
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
