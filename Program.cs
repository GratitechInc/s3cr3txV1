using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using s3cr3tx.Models;
using static s3cr3tx.Controllers.ebundle;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "s3cr3tx API",
        Version = "v1",
        Description = "An API to perform secret protection operations",
        TermsOfService = new Uri("https://www.github.com/GratitechInc/s3cr3tx"),
        Contact = new OpenApiContact
        {
            Name = "Patrick Kelly",
            Email = "Patrick@Gratitech.com",
            Url = new Uri("https://LinkedIn.com/in/PatrickAgileAppSecInfoSec"),
        }
    });
    //c.OperationFilter<CustomHeaderSwaggerAttribute>();
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddRazorPages();
builder.Services.AddMvcCore();
builder.Services.AddDbContext<S3cr3txDbContext>();


var app = builder.Build();
//using (var scope = app.Services.CreateScope())
//{
//    var service = scope.ServiceProvider;
//    var context = service.GetService<s3cr3tx.Models.S3cr3txDbContext>();

//}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.MapRazorPages();

app.Run();
