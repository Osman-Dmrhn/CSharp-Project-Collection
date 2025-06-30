using BlogAngApi.Db;
using BlogAngApi.Service;
using BlogAngApi.Services;
using BlogApplication.Areas.AdminArea.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IAdminServices, AdminService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],   // JWT Issuer
            ValidAudience = builder.Configuration["JwtSettings:Audience"], // JWT Audience
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])) // Secret Key
        };
    });


builder.Services.AddAuthentication()
    .AddCookie("AdminAuth", options =>
    {
        options.LoginPath = "/AdminArea/Admin/Login"; // Admin login page
        options.AccessDeniedPath = "/AdminArea/Admin/AccessDenied";
        options.SlidingExpiration = true;
        options.Cookie.Name = "AdminAuthCookie";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Set a reasonable expiration time
    })
    .AddCookie("user", options =>
    {
        options.LoginPath = "/Login/LoginView"; // Normal user login page
        options.AccessDeniedPath = "/Login/LoginView";
        options.SlidingExpiration = true;
        options.Cookie.Name = "UserAuthCookie";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Set a reasonable expiration time
    });

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim(ClaimTypes.Role, "Admin");
    });

    options.AddPolicy("UserOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim(ClaimTypes.Role, "User");
    });
});

// Configure Entity Framework
builder.Services.AddDbContext<EfContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:7168", "http://localhost:4200") 
              .AllowAnyMethod()                     
              .AllowAnyHeader()                    
              .AllowCredentials();                  
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllowLocalhost");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();

app.Run();
