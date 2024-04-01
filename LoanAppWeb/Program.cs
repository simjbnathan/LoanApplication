using LoanAppWeb.Models;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
services.AddHttpClient("LoanAppApi", client =>
{
    var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();   
    client.BaseAddress = new Uri(apiSettings.LoanApiBaseUrl);
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
