using ETicaret.Application;
using ETicaret.Application.Features.Commands.Product.CreateProduct;
using ETicaret.Application.Validators.Products;
using ETicaret.Application.ViewModels.Products;
using ETicaret.Infrastructure;
using ETicaret.Infrastructure.Services.Storage.Local;
using ETicaret.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();

builder.Services.AddControllers();
    //.ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<CreateProductCommandRequest>, CreateProductValidator>();


builder.Services.AddInfrastructureServices();
builder.Services.AddStorage<LocalStorage>();

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
                .AllowAnyMethod();
        });
});

var app = builder.Build();


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

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
 