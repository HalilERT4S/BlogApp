using BlogApp.Application.Interfaces.Repositories;
using BlogApp.Application.Interfaces.Services;
using BlogApp.Application.Models;
using BlogApp.Application.Services;
using BlogApp.Application.Validators;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Repositories;
using BlogApp.Web.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlogApp.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using BlogApp.Web.Filters;
using BlogApp.Infrastructure.Configuration;
using BlogApp.Domain.Interfaces; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


builder.Services.AddScoped<IOpenAIService>(provider =>
{
    var config = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<AppSettings>>().Value;
    return new OpenAIService(config.OpenAIKey);
});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IBlogValidateService,BlogService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryValidateService,CategoryService>();
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<IValidator<UserRegistrationModel>, UserRegistrationModelValidator>();
builder.Services.AddScoped<IValidator<UserLoginModel>, UserLoginModelValidator>();
builder.Services.AddScoped<IValidator<BlogCreateDto>, BlogCreateDtoValidator>();
builder.Services.AddScoped<IValidator<BlogUpdateDto>, BlogUpdateDtoValidator>();
builder.Services.AddScoped<IValidator<CategoryCreateDto>,CategoryCreateDtoValidator>();
builder.Services.AddScoped<IValidator<CategoryUpdateDto>, CategoryUpdateDtoValidator>();

builder.Services.AddScoped<AuthorizationFilter>();


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

builder.Services.AddAutoMapper(typeof(BlogApp.Application.Mappings.CategoryMappingProfile).Assembly);

builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();


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
//         Description = "JWT Token'ýnýzý girin.(Bearer {token})"
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


if (app.Environment.IsDevelopment())
{
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

app.UseSession();
app.UseRouting();

app.UseSessionCheck();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Blog}/{action=BlogList}/{id?}");

app.Run();
