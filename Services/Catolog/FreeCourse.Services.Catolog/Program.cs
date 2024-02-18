using FreeCourse.Services.Catolog.Services;
using FreeCourse.Services.Catolog.Settings;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers(opt => {
    //t�m controller� auhroize attr eklemek i�in kullan�l�r
    opt.Filters.Add(new AuthorizeFilter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<IDatabaseSettings>(sp => {
    //GetRequiredService -> ilgili servisi bulamazsa hata f�rlat�r
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    //herhangi bir class ctor'�nda IDatabaseSettings ge�tipi anda dolu bir databasesetting verisi gelecektir.
});

//mikroservisi koruma alt�na alma
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.Authority = builder.Configuration["IdentityServerURL"]; //bu mikroservise token� kim da��t�tyor 
    options.Audience = "resource_catalog"; //birden fazla belirtilemiyor 
    options.RequireHttpsMetadata = false; //default https bekledi�i i�in false set ediyoruz
});


builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICourseService, CourseService>();

builder.Services.AddMassTransit(x =>
{
    // Default Port : 5672
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQUrl"], "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
