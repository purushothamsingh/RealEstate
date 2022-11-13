
using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Models;
using RealEstateAPI.Helper;
using RealEstateAPI.Repositories.LoginRepo;
using RealEstateAPI.Repositories.PropertyRepo;

using RealEstateAPI.Repositories.PhotoRepo;
using RealEstateAPI.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RealEstateAPI.Repositories.WishRepo;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("Myconn")));
builder.Services.AddScoped<IAuthRepo, Auth>();
builder.Services.AddScoped<ICityRepo, CityRepo>();
builder.Services.AddScoped<IPropertyRepo, PropertyRepo>();
builder.Services.AddScoped<IPropertyTypeRepo, PropertyTypeRepo>();
builder.Services.AddScoped<IFurnishingTypeRepo, FurnishingTypeRepo>();

builder.Services.AddScoped<IPhotoService, PhotoService>();

builder.Services.AddScoped<IWishRepo, Wish>();
var secretKey = "mytoken idkaldkhodsildbjafso";
var key = new SymmetricSecurityKey(Encoding.UTF8
    .GetBytes(secretKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               // services.AddAuthentication("Bearer")
               .AddJwtBearer(opt => {
                   opt.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       IssuerSigningKey = key
                   };
               });


builder.Services.AddCors(
    (options) =>
    {
        options.AddPolicy("default", (option) =>
        {
            option.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader();
        });
    }
    );

builder.Logging.AddLog4Net();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("default");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();
