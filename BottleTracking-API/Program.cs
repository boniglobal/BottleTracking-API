using BottleTracking_API.Dependencies;
using BottleTracking_API.Helpers;
using Business.Dependency;
using Core.Utilities.Encryption;
using Core.Utilities.JWT;
using Data.Concrete.Contexts;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ValidateModelStateAttribute));
}).AddFluentValidation();

builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddDbContext<BottleTrackingDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"))
       .UseSnakeCaseNamingConvention();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "DefaultPolicy",
                      builder =>
                      {
                          builder.SetIsOriginAllowed(origin => true)
                                 .AllowAnyHeader()
                                 .AllowAnyMethod()
                                 .AllowCredentials();
                      });
});

builder.Services.AddOptions<TokenSettings>().Bind(builder.Configuration.GetSection(nameof(TokenSettings)));
builder.Services.AddOptions<EncryptionSettings>().Bind(builder.Configuration.GetSection(nameof(EncryptionSettings)));

builder.Services.AddCustomServices();
builder.Services.AddRouting(opt => opt.LowercaseUrls = true);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var tokenSettings = builder.Configuration.GetSection(nameof(TokenSettings)).Get<TokenSettings>();
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = tokenSettings.Issuer,
        ValidAudience = tokenSettings.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Secret)),
        ClockSkew = TimeSpan.Zero
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfigurations();

builder.Services.AddSerilogConfigurations(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}



app.UseHttpsRedirection();

app.UseCors("DefaultPolicy");

app.UseMiddleware<RequestLogContextMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ErrorHandler>();
app.UseMiddleware<AuthHandler>();

app.MapControllers();

app.Run();
