var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Use your middleware after routing but before endpoints
app.UseMiddleware<ApiProxyMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "webhook",
        pattern: "api/webhook",
        defaults: new { controller = "Webhook", action = "HandleWebhook" });

    endpoints.MapControllerRoute(
        name: "notifications",
        pattern: "notifications/{action=Index}",
        defaults: new { controller = "Notifications" });
});

app.Run();
