using Services.Dependecy;
using DataAccessLayer.Dependency;
using Entities;
using Microsoft.AspNetCore.Identity;
using DataAccessLayer.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataAccessLayer.Persistance.Seed;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Common.ViewModels;
using Newtonsoft.Json;
using Microsoft.Extensions.FileProviders;
using Hangfire;
using Services.Interface;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
services.AddServices(configuration);
services.AddDataLinkLayerServices(configuration);

builder.Services.AddIdentity<Users, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin();
        //builder.WithOrigins("https://localhost:").WithHeaders("X-API-Verisons");
    });
});
// Adding Authentication  
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer  
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWTSettings:Audience"],
        ValidIssuer = builder.Configuration["JWTSettings:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:Key"]?? throw new NullReferenceException())),
        ValidateIssuerSigningKey=true
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseStaticFiles(
    new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            configuration.GetValue<string>("ProjectLocation") ?? throw new InvalidOperationException("Image Location Not Found")),
        RequestPath = "/MyImages"
    }
);



app.UseExceptionHandler(
    options =>
    {
        options.Run(async context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();
            if (null != exceptionObject)
            {
                var res = new Response<dynamic>
                {
                    Status = "Error",
                    Message = exceptionObject.Error.Message,
                    HttpStatus = HttpStatusCode.InternalServerError
                };
                var jsonResponse = JsonConvert.SerializeObject(res);
                await context.Response.WriteAsync(jsonResponse).ConfigureAwait(false);
            }
        });
    }
);
app.UseHttpsRedirection();

app.UseAuthorization();


// to implement cors
app.UseCors();


app.UseHangfireDashboard();
app.MapHangfireDashboard();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await dbInitializer.Initialize();
}

RecurringJob.AddOrUpdate<IBackGroundServices>(x => x.SendNotificationMovieRelease(), cronExpression: "0 0 * * *");


app.Run();
