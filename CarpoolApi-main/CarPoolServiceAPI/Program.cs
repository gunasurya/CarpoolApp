using CarpoolService.BLL.Services;
using CarpoolService.DAL.Repositories;
using CarPoolService.Contracts.Interfaces.Repository_Interfaces;
using CarPoolService.Contracts.Interfaces.Service_Interface;
using CarPoolService.DAL;
using CarPoolServiceAPI.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CarpoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CarpoolCS")));
builder.Services.AddCors(cors => cors.AddPolicy("MyPolicy", policy =>
{
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddAuthentication(configureOptions: options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});
builder.Services.AddScoped<IUserService, UserService>();  
builder.Services.AddScoped<IBCryptService, BCryptAdapterService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRideRepository, RideRepository>();
builder.Services.AddScoped<IRideService, RideService>();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(MappingProfile), typeof(Program));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CarPoolServiceAPI", Version = "v1" });

    // Add the JWT Bearer security scheme
    var securityScheme = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    // Make sure the Swagger UI requires a Bearer token to authorize requests
    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting(); // Add routing middleware
app.UseCors("MyPolicy"); // Enable CORS
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
