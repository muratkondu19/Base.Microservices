using FreeCourse.Gateway.DelegateHandlers;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;


var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile($"configuration.{builder.Environment.EnvironmentName}.json", true, true)
                            .Build();

//builder.Services.AddOcelot(configuration);
builder.Services.AddHttpClient<TokenExhangeDelegateHandler>();

builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", options => {
    options.Authority = builder.Configuration["IdentityServerURL"];
    options.Audience = "resource_gateway";
    options.RequireHttpsMetadata = false;
});

builder.Services.AddOcelot().AddDelegatingHandler<TokenExhangeDelegateHandler>();

var app = builder.Build();



app.UseHttpsRedirection();
app.UseOcelot().Wait();
app.Run();