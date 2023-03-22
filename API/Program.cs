using System.Text;
using System.Text.Json.Serialization;
using API;
using API.Extensions;
using Azure.Storage.Blobs;
using Constants;
using DAL;
using Microsoft.EntityFrameworkCore;
using Domain.Identity;
using Interfaces.Base;
using Interfaces.Converters;
using Interfaces.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PublicAPI.DTO.MappingProfiles;
using Repositories.Base;
using Services.Base;
using Services.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("NpgsqlConnection");
var azureConnection = builder.Configuration.GetConnectionString("AzureBlobStorageConnectionString");
                       
//DB
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddMemoryCache();


//defaultScheme: JwtBearerDefaults.AuthenticationScheme

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ClockSkew = TimeSpan.Zero // remove delay of token when expire
        };
        
        cfg.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments(HubPaths.LiveGameHub, StringComparison.InvariantCulture)))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddDerived(typeof(IBaseRepository<>), typeof(Repository<>), ServiceLifetime.Scoped);
builder.Services.AddDerived(typeof(IBaseService), typeof(Service), ServiceLifetime.Scoped);
builder.Services.AddDerived(typeof(IConverter<,>), typeof(Converters.Converter<,>), ServiceLifetime.Scoped);
builder.Services.AddSingleton(x => new BlobServiceClient(azureConnection));

builder.Services.AddScoped<ILiveGameHubContext, LiveGameHubContext>();


builder.Services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>();



builder.Services.AddMvc().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; });

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsAllowAll", policyBuilder =>
    
        policyBuilder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(hostName => true)
   
    );
});

builder.Services.AddSignalR().AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});


    builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 0;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.

AppDataHelper.SetUpAppData(app, builder.Configuration);

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("CorsAllowAll");

app.UseRouting();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.MapControllers();

app.MapHub<LiveGameHub>(HubPaths.LiveGameHub);

app.Run();