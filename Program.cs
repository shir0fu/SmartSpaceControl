using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartSpaceControl.Data;
using Microsoft.OpenApi.Models;
using SmartSpaceControl.Models.Models;
using SmartSpaceControl.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using SmartSpaceControl.Repository;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


var test = builder.Configuration.GetValue<string>("JwtConfig:Key");

#region Configure App Services

builder.Services.AddCors();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddSignInManager<SignInManager<User>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);
builder.Services.AddMvc();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartSpaceControlAPI", Version = "v1" });
});
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "https://localhost:7187",
        ValidAudience = "SmartSpaceControl",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(test))
    };
});
builder.Services.AddAuthorization();

#endregion

#region Dependency Injection

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IAreaService, AreaService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

#endregion

var app = builder.Build();
app.UseCors(builder => builder.AllowAnyOrigin()
                             .AllowAnyHeader()
                            .AllowAnyMethod());
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartSpaceControlAPI");
    c.RoutePrefix = "";
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();