
using AppSec__practicalAssignment_.Models;
using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AuthenDbContext>();

builder.Services.AddIdentity<UserClass, IdentityRole>()
    .AddEntityFrameworkStores<AuthenDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddReCaptcha(builder.Configuration
    .GetSection("GoogleReCaptcha"));

builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
{
	options.Cookie.Name = "MyCookieAuth";
	options.AccessDeniedPath = "/error/403";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBelongToHRDepartment", policy => policy.RequireClaim("Department", "HR"));
});

builder.Services.ConfigureApplicationCookie(
    Config => { Config.LoginPath = "/Login"; }
);

builder.Services.AddDistributedMemoryCache(); //save session in memory

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();  

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseStatusCodePagesWithReExecute("/error/{0}"); 

app.UseSession();  

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
