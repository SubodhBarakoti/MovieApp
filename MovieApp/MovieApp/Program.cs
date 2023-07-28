using DataAccessLayer.Persistance;
using Microsoft.AspNetCore.Identity;
using Services.Dependecy;
using DataAccessLayer.Dependency;
using DataAccessLayer.Persistance.Seed;
using Entities;
using Microsoft.Extensions.FileProviders;
using Hangfire;
using Services.Interface;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
services.AddServices(configuration);
services.AddDataLinkLayerServices(configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<Users, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();

builder.Services.AddControllersWithViews();
builder.Services.AddMvc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(
    new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            configuration.GetValue<string>("ProjectLocation") ?? throw new InvalidOperationException("Image Location Not Found")),
        RequestPath="/MyImages"
    }
);

app.UseRouting();

app.UseAuthorization();

app.UseHangfireDashboard();
app.MapHangfireDashboard();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movies}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await dbInitializer.Initialize();
}

RecurringJob.AddOrUpdate<IBackGroundServices>(x => x.SendNotificationMovieRelease(), cronExpression: "0 0 * * *");

app.Run();
