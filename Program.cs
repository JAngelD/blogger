

using bloggers.Services;
using System.Runtime.InteropServices;
using bloggers.Models;
using bloggers.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;
//using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using bloggers.UnitOfWork.Interfaces;
using bloggers.UnitOfWork.Services;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration _configuration = builder.Build();
var _builder = WebApplication.CreateBuilder();
_builder.Configuration.AddConfiguration(_configuration);
    

ConfigureServices();


var app = _builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // app.UseExceptionHandler("/Errors/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

// Global error handler
// app.UseMiddleware<JwtMiddleware>();
// app.UseMiddleware<VisitasMiddleware>();
// app.UseMiddleware<ErrorHandlerMiddleWare>();
app.UseAuthentication();
app.UseAuthorization();
 app.UseDeveloperExceptionPage();
// Global Cors policy
app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();

void ConfigureServices()
{
    var env     = _builder.Environment;
    _builder.Services.AddCors();


    // registar servicios usados en paginas y controllers
    _builder.Services.AddControllersWithViews().AddJsonOptions(x =>
    {
        // TODO arreglar problema que permite injeccion de scripts
        var encoderSettings = new TextEncoderSettings();
        encoderSettings.AllowCharacters('\u0436', '\u0430');
        encoderSettings.AllowRange(UnicodeRanges.BasicLatin);
        x.JsonSerializerOptions.Encoder =  JavaScriptEncoder.Create(encoderSettings);
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

    // configura DI for application services
    _builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    
    _builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    _builder.Services.AddScoped<IBloggerService,BloggerService>();
    _builder.Services.AddScoped<IFriendService,FriendService>();
    
    // Configure Automapper in the project
    _builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    _builder.Services.AddAutoMapper(typeof(StartupBase));

    string blog = _configuration.GetConnectionString("angel");
    _builder.Services.AddDbContext<BloggerTestContext>(options => {
            options.UseSqlServer(blog);
            options.EnableSensitiveDataLogging();
        }, ServiceLifetime.Transient);
    _builder.Services.AddSession();
}
