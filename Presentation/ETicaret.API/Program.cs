using ETicaret.API.Configurations.ColumnWriters;
using ETicaret.API.Extensions;
using ETicaret.API.Middlewares;
using ETicaret.Application;
using ETicaret.Application.Features.Commands.Product.CreateProduct;
using ETicaret.Application.Validators.Products;
using ETicaret.Application.ViewModels.Products;
using ETicaret.Infrastructure;
using ETicaret.Infrastructure.Services.Storage.Local;
using ETicaret.Persistence;
using ETicaret.SignalR;
using ETicaret.SignalR.Hubs;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using System.Text;
using System.Text.Unicode;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();
builder.Services.AddSignalRServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
    //.ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<CreateProductCommandRequest>, CreateProductValidator>();
builder.Services.AddInfrastructureServices();
builder.Services.AddStorage<LocalStorage>();


Logger log = new LoggerConfiguration()
                 .WriteTo.Console()
                 .WriteTo.File("logs/log.txt")
                 .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSql"),"logs",needAutoCreateTable:true,
                  columnOptions: new Dictionary<string, ColumnWriterBase>
                  {
                      {"message",new RenderedMessageColumnWriter() },
                      {"message_template",new MessageTemplateColumnWriter() },
                      {"level",new LevelColumnWriter() },
                      {"time_stamp",new TimestampColumnWriter() },
                      {"exception",new ExceptionColumnWriter() },
                      {"log_event", new LogEventSerializedColumnWriter() },
                      {"user_name",new UsernameColumnWriter() }
                  })
                  .WriteTo.Seq(builder.Configuration["Seq:ServerURL"])
                  .Enrich.FromLogContext()
                  .MinimumLevel.Information()
                 .CreateLogger();

builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddTransient<UserNameMiddleware>();

builder.Services.AddAplicationServices();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("Admin",opt =>
                {
                    opt.TokenValidationParameters = new()
                    {
                        ValidateAudience = true, //token deðerinin kullanýcýsý => www.bilmemne.com
                        ValidateIssuer = true, // token deðerini daðýtan => www.myapi.com
                        ValidateLifetime = true, // token süresi
                        ValidateIssuerSigningKey = true, // token deðerinin uygulamamýza ait olduðunu ifade eden security key

                        ValidAudience = builder.Configuration["Token:Audience"],
                        ValidIssuer = builder.Configuration["Token:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
                        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                        {
                            return expires != null ? expires > DateTime.UtcNow : false;
                        },

                        NameClaimType = ClaimTypes.Name 
                    };
                });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod().AllowCredentials();
        });
});

var app = builder.Build();

app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILogger<Program>>());

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseStaticFiles(new StaticFileOptions
{
    RequestPath = "/staticfile"
}); 

app.UseSerilogRequestLogging();
app.UseHttpLogging();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<UserNameMiddleware>();

app.MapControllers();

app.MapHubs();

app.Run();
 