using System.Text;
using Elastic.Apm.NetCoreAll;
using JobStreamline.Entity;
using JobStreamline.DataAccess;
using JobStreamline.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Hangfire;
using JobStreamline.Api;
using Hangfire.PostgreSql;
using StackExchange.Redis;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
       {
           options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
           options.JsonSerializerOptions.PropertyNamingPolicy = null;
       });


var securityScheme = new OpenApiSecurityScheme
{
    Description = "JWT Authorization header using the Bearer scheme.",
    Name = "JWT",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.Http,
    Scheme = "bearer"
};

var securityReq = new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "JWT"
            }
        },
        new string[] {}
    }
};

var basicSecurityScheme = new OpenApiSecurityScheme
{
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    Scheme = "Basic",
    In = ParameterLocation.Header,
    Description = "Basic Authorization header using the Bearer scheme."
};

var basicSecurityReq = new OpenApiSecurityRequirement
{
    {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Basic"
                }
            },
            new string[] {}
    }
};

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        RequireExpirationTime = true
    };
});

builder.Services.AddAuthorization();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("JWT", securityScheme);
    o.AddSecurityRequirement(securityReq);
    o.SupportNonNullableReferenceTypes();
});

builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnectionString")));

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddDbContext<JobStreamlineDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IElasticsearchService, ElasticsearchService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddSingleton<IBlackwordService, BlackwordService>();

builder.Services.AddHangfire(x =>
 x.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();


using (var context = new JobStreamlineDbContext(builder.Configuration))
{
    context.Database.Migrate();
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();
    await jobService.CreateIndex();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (Convert.ToBoolean(builder.Configuration["IsUseElasticApm"]) == true)
{
    app.UseAllElasticApm(builder.Configuration);
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/hangfire/dashboard", new DashboardOptions
{
    DashboardTitle = "JobStreamline Hangfire Dashboard",
    DarkModeEnabled = true,
});

app.StartJobs(builder.Services.BuildServiceProvider(), builder.Configuration);

app.Run();