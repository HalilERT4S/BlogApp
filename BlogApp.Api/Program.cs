using BlogApp.Web.Services;
using BlogApp.Application.Interfaces.Repositories;
using BlogApp.Application.Interfaces.Services;
using BlogApp.Application.Models;
using BlogApp.Application.Services;
using BlogApp.Application.Validators;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Veritaban� ba�lant�s�
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection kay�tlar�
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IValidator<UserRegistrationModel>, UserRegistrationModelValidator>();
builder.Services.AddScoped<IValidator<UserLoginModel>, UserLoginModelValidator>();
// BlogCreateDto ve BlogUpdateDto'yu MVC taraf�nda kullanacaksan�z kay�tlar� tutun.
// E�er sadece ViewModel kullanacaksan�z bu kay�tlar� kald�rabilirsiniz.
builder.Services.AddScoped<IValidator<BlogCreateDto>, BlogCreateDtoValidator>();
builder.Services.AddScoped<IValidator<BlogUpdateDto>, BlogUpdateDtoValidator>();

// Kimlik Do�rulama (JWT kalabilir, MVC ile de kullan�labilir)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });

builder.Services.AddAuthorization();

// MVC servislerini ekle
builder.Services.AddControllersWithViews();

// API Explorer ve Swagger'� MVC projesinde kullanmayacaksan�z kald�r�n
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog API", Version = "v1" });
//
//     c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         Name = "Authorization",
//         Type = SecuritySchemeType.ApiKey,
//         Scheme = "Bearer",
//         BearerFormat = "JWT",
//         In = ParameterLocation.Header,
//         Description = "JWT Token'�n�z� girin. �rnek: Bearer {token}"
//     });
//
//     c.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference
//                 {
//                     Type = ReferenceType.SecurityScheme,
//                     Id = "Bearer"
//                 }
//             },
//             Array.Empty<string>()
//         }
//     });
// });

var app = builder.Build();

// Geli�tirme ortam�
if (app.Environment.IsDevelopment())
{
    // Swagger ve Swagger UI'y� MVC projesinde kullanmayacaksan�z kald�r�n
    // app.UseSwagger();
    // app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();