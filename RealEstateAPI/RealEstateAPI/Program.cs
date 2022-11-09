
using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Models;
using RealEstateAPI.Helper;
using RealEstateAPI.Repositories.LoginRepo;
using RealEstateAPI.Repositories.PropertyRepo;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("Myconn")));
builder.Services.AddScoped<IAuthRepo, Auth>();
builder.Services.AddScoped<ICityRepo, CityRepo>();
builder.Services.AddScoped<IPropertyRepo, PropertyRepo>();
builder.Services.AddCors(
    (options) =>
    {
        options.AddPolicy("default", (option) =>
        {
            option.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader();
        });
    }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("default");
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();



app.Run();
