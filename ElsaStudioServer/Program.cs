using Elsa.Studio.Backend.Extensions;
using Elsa.Studio.Core.BlazorServer.Extensions;
using Elsa.Studio.Dashboard.Extensions;
using Elsa.Studio.Shell.Extensions;
using Elsa.Studio.Workflows.Extensions;
using Elsa.Studio.Extensions;
using Elsa.Studio.Login.Extensions;
using Elsa.Studio.Workflows.Designer.Extensions;
using ElsaStudioServer;
using Elsa.Studio.Contracts;
using Elsa.Studio.Login.Contracts;
using Elsa.Studio.Login.Services;

// Build the host.
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Register Razor services.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(options =>
{
    // Register the root components.
    options.RootComponents.RegisterCustomElsaStudioElements();
});

// Register shell services and modules.
builder.Services.AddShell();
builder.Services.AddBlazorServerModule();
builder.Services.AddKeycloak(builder.Configuration.GetSection("Oidc"));
builder.Services.AddDataProtection();
//builder.Services.AddDefaultAuthentication();
builder.Services.AddBackendModule(options => configuration.GetSection("Backend").Bind(options));
builder.Services.AddScoped<ILoginPageProvider, ElsaStudioServer.LoginPageProvider>();
builder.Services.AddScoped<ICredentialsValidator, DefaultCredentialsValidator>();
//builder.Services.AddLoginModule();
builder.Services.AddDashboardModule();
builder.Services.AddWorkflowsModule();

// Configure SignalR.
builder.Services.AddSignalR(options =>
{
    // Set MaximumReceiveMessageSize:
    options.MaximumReceiveMessageSize = 5 * 1024 * 1000; // 5MB
});

// Build the application.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseResponseCompression();
    
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Run the application.
app.Run();