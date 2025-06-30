using BlogApplication.Areas.AdminArea.Service;
using BlogApplication.Db;
using BlogApplication.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "user";  
})
.AddCookie("AdminAuth", options =>
{
    options.LoginPath = "/AdminArea/Admin/Login";
    options.AccessDeniedPath = "/AdminArea";
    options.Cookie.MaxAge = null; 
    options.ExpireTimeSpan = TimeSpan.FromMinutes(0);
    options.SlidingExpiration = true;
})
.AddCookie("user", options =>
{
    options.LoginPath = "/Login/LoginView";  
    options.LogoutPath = "/Login/Logout";
    options.AccessDeniedPath = "/Login/LoginView";
});


builder.Services.AddAuthorization();


builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IBlogService,BlogService>();
builder.Services.AddScoped<IAdminServices,AdminService>();
builder.Services.AddScoped<ICommentService,CommentService>();
builder.Services.AddScoped<ICategoryService,CategoryService>();


builder.Services.AddDbContext<EfContext>();



var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  
app.UseAuthorization();   

app.MapControllerRoute(
    name: "admin_area",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

app.MapDefaultControllerRoute();  // Diðer rotalar için varsayýlan ayar

app.MapControllers();

app.Run();
