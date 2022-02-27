using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);
IdentityModelEventSource.ShowPII = true;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration); ;

builder.Services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
     
    };

    // for debugging auth events
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = (c) => {
            return Task.CompletedTask;        
        },

        OnChallenge = (c) => {
            return Task.CompletedTask;
        },

        OnForbidden = (context) => { 

            return Task.CompletedTask;
        },
        OnAuthenticationFailed = (context) =>
        {

            return Task.CompletedTask;
        }
    };
    //options.Audience = "web api's client id";
    //options.Authority = "https://login.microsoftonline.com/tenantid/v2.0";
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// this is very important Authentication then Authorization
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
