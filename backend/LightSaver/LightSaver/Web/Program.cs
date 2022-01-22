using LightSaver.Domain.Services;
using LightSaver.Infrastructure;
using LightSaver.Infrastructure.DAO;
using LightSaver.Services;
using Microsoft.EntityFrameworkCore;


//EQUIVALENT TO CONFIGURE SERVICES

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyDbContext>(options => options.UseMySql("server = localhost; user = root; database = lightSaver; password = 1234; port = 3306", new MySqlServerVersion(new Version(10, 1, 40))));


//The creation of the DB cannot be in the DAO since it is dependency injected and each time, a new instance is created so the DB would be attempting to create each time a call to the DB is required
//Creation of the database if it does not exists
var context = builder.Services.BuildServiceProvider().GetService<MyDbContext>();
context.Database.EnsureCreated();



//adding HTTPCLIENT
builder.Services.AddHttpClient<REEService>();
builder.Services.AddHttpClient<IFTTTService>();

//adding Background Service Refesh Prices
builder.Services.AddHostedService<RefreshPricesService>(); //Keypoint in here is that it runs in another thread parallel to the main thread
//builder.Services.AddSingleton<RefreshPricesService>();
//builder.Services.AddTransient<RefreshPricesService>();

builder.Services.AddTransient<PricesDAO>();
builder.Services.AddTransient<PricesService>();
//They must be singleton since if not DB is attempted to be created twice

//builder.Services.AddSingleton<PricesDAO>();
//builder.Services.AddSingleton<PricesService>();







//EQUIVALENT TO CONFIGURE

var app = builder.Build();


// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Manually starting the Refreshprice service
//app.Lifetime.ApplicationStarted.Register(()=>app.Services.GetService<RefreshPricesService>()?.Start());
//app.Lifetime.ApplicationStopped.Register(()=>app.Services.GetService<RefreshPricesService>()?.Stop());

app.Run();
