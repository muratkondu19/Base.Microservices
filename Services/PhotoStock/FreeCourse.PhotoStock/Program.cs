using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt => {
    //tüm controllerý auhroize attr eklemek için kullanýlýr
    opt.Filters.Add(new AuthorizeFilter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//mikroservisi koruma altýna alma
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.Authority = builder.Configuration["IdentityServerURL"]; //bu mikroservise tokený kim daðýtýtyor 
    options.Audience = "resource_catalog"; //birden fazla belirtilemiyor 
    options.RequireHttpsMetadata = false; //default https beklediði için false set ediyoruz
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {

}

//dosyalarý dýþ dünyaya açma wwwroot içerisindeki dosyalara public olarak dýþarýdan eriþilir 
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
