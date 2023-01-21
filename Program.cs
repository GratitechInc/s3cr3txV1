using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using s3cr3tx.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
