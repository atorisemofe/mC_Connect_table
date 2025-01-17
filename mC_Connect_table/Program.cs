using mC_Connect_table.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using mC_Connect_table.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RestaurantTablesContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("RestaurantTablesContext") ?? throw new InvalidOperationException("Connection string 'RestaurantTablesContext' not found.")));
builder.Services.AddDbContext<OrderViewContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("OrderViewContext") ?? throw new InvalidOperationException("Connection string 'OrderViewContext' not found.")));
    
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddDistributedMemoryCache(); // Adds in-memory cache
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSignalR(); // Add SignalR services
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor(); 

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseSession(); // Add session middleware


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Use your middleware after routing but before endpoints
app.UseMiddleware<ApiProxyMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=WebApi}/{id?}");

// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllerRoute(
//         name: "default",
//         pattern: "{controller=Home}/{action=WebApi}");

//     endpoints.MapControllerRoute(
//         name: "webhook",
//         pattern: "api/webhook",
//         defaults: new { controller = "Webhook", action = "HandleWebhook" });

//     endpoints.MapControllerRoute(
//         name: "notifications",
//         pattern: "notifications/{action=Index}",
//         defaults: new { controller = "Notifications" });

//     endpoints.MapControllerRoute(
//         name: "front",
//         pattern: "front/{action=Index}",
//         defaults: new { controller = "Front" });

//     endpoints.MapControllerRoute(
//         name: "menu",
//         pattern: "menu/{action=Index}",
//         defaults: new { controller = "Menu" });

//     endpoints.MapControllerRoute(
//         name: "back",
//         pattern: "back/{action=Index}",
//         defaults: new { controller = "Back" });
// });

app.MapHub<NotificationHub>("/notificationHub"); // Map the SignalR hub


app.Run();
