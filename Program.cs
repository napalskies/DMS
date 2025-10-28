using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyDMS.Application;
using MyDMS.Application.FileStorage;
using MyDMS.Domain;
using MyDMS.Infrastructure;
using MyDMS.Infrastructure.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//add services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = PathString.Empty;  // Prevents redirection to /Account/Login
    options.AccessDeniedPath = PathString.Empty;  // Prevents redirection for forbidden requests

    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});
var allowedOrigin = "https://napalskies.github.io";

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(allowedOrigin) 
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); 
    });
});

builder.Services.AddControllers(); // forces the app to search the codebase for any controllers we might have
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configure services added in the app
builder.Services.AddScoped<TokenService>();
//builder.Services.AddSingleton<EncryptionService>();
var configuration = builder.Configuration;
DMSConfig.Initialize(configuration);

builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IFileStorageService, DbFileStorageService>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<FolderRepository>();
builder.Services.AddScoped<FolderService>();

var tokenSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true, 
            ValidateIssuerSigningKey = true,
            ValidIssuer = tokenSettings["Issuer"] ?? "",
            ValidAudience = tokenSettings["Audience"] ?? "",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings["Key"] ?? "")),
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin"));
});



var app = builder.Build();

app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.Use(async (context, next) =>
//{
//    var user = context.User;
//    Console.WriteLine($"User authenticated: {user.Identity?.IsAuthenticated}");
//    Console.WriteLine($"User roles: {string.Join(", ", user.Claims.Where(c => c.Type.Contains("role")).Select(c => c.Value))}");
//    await next();
//});

//app.Use(async (context, next) =>
//{
//    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
//    await next();
//    Console.WriteLine($"Response: {context.Response.StatusCode}");
//});

//app.MapGet("/enc", (EncryptionService encryptionService) =>
//{
//    return encryptionService.Encrypt("Hello, World!");
//});


//app.MapGet("/dec", (EncryptionService encryptionService) =>
//{
//    return encryptionService.Decrypt(encryptionService.Encrypt("Hello, World!"));
//});
app.Run();
