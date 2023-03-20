using DemoDynamicRolePolicy.AuthPolicy;
using DemoDynamicRolePolicy.DBContext;
using DemoDynamicRolePolicy.IdentityAuth;
using DemoDynamicRolePolicy.Interface;
using DemoDynamicRolePolicy.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using ZendeskApi_v2.Models.CustomRoles;

var Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddControllers();

builder.Services.AddCors(options => options.AddPolicy("AllowOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200", "http://localhost:5286").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddTransient<IRoleRepository, RoleRepository>();
builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IAuthorizationHandler, ShouldBeAnAdminRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ShouldBeAnUserRequirementHandler>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DemoDynamicRolePolicy",
        Version = "v1"
    });

    c.AddSecurityDefinition("token", new OpenApiSecurityScheme
    {

        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = HeaderNames.Authorization,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="token"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("AdminViewData", options =>
    {
        options.RequireAuthenticatedUser();
        options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        options.Requirements.Add(new ShouldBeAReaderRequirement("policyName"));
    });

    config.AddPolicy("AdminCreate", options =>
    {
        options.RequireAuthenticatedUser();
        options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        options.Requirements.Add(new ShouldBeAReaderRequirement("policyName"));
    });

    config.AddPolicy("AdminEdit", options =>
    {
        options.RequireAuthenticatedUser();
        options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        options.Requirements.Add(new ShouldBeAReaderRequirement("policyName"));
    });

    config.AddPolicy("AdminDelete", options =>
    {
        options.RequireAuthenticatedUser();
        options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        options.Requirements.Add(new ShouldBeAReaderRequirement("policyName"));
    });

    config.AddPolicy("UserViewData", options =>
    {
        options.RequireAuthenticatedUser();
        options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        options.Requirements.Add(new ShouldBeUserRequirement("policyName"));
    });

    config.AddPolicy("UserCreate", options =>
    {
        options.RequireAuthenticatedUser();
        options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        options.Requirements.Add(new ShouldBeUserRequirement("policyName"));
    });

    config.AddPolicy("UserEdit", options =>
    {
        options.RequireAuthenticatedUser();
        options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        options.Requirements.Add(new ShouldBeUserRequirement("policyName"));
    });

    config.AddPolicy("UserDelete", options =>
    {
        options.RequireAuthenticatedUser();
        options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        options.Requirements.Add(new ShouldBeUserRequirement("policyName"));
    });

});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(optioins =>
{
    optioins.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    optioins.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    optioins.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    optioins.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    optioins.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "Url");
    options.CallbackPath = "/signin-google";
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
    };
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowOrigin");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")),
    RequestPath = new PathString("/Images")
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
