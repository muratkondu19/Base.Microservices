using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt => {
    //t�m controller� auhroize attr eklemek i�in kullan�l�r
    opt.Filters.Add(new AuthorizeFilter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//mikroservisi koruma alt�na alma
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.Authority = builder.Configuration["IdentityServerURL"]; //bu mikroservise token� kim da��t�tyor 
    options.Audience = "resource_catalog"; //birden fazla belirtilemiyor 
    options.RequireHttpsMetadata = false; //default https bekledi�i i�in false set ediyoruz
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {

}

//dosyalar� d�� d�nyaya a�ma wwwroot i�erisindeki dosyalara public olarak d��ar�dan eri�ilir 
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
