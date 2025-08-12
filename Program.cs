using System.Configuration;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using LogCap.Model;

var builder = WebApplication.CreateBuilder(args);

// Configuring ForwardHeaders so we can get IP address when runnnig NGINX
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins",
        builder =>
        {
            builder.WithOrigins("https://newlibre.com", "https://allos.dev", "https://cyapass.com")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseCors("AllowedOrigins");
// We only need UseForwardHeaders when running on Linux behind NGINX - to get ip addresses
if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux)){
    Console.WriteLine("Running on on Linux...using ForwardHeaders");
    app.UseForwardedHeaders();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/Save/{SiteDesc}/{RefUrl?}/{Info?}", async(HttpContext context, String SiteDesc, String RefUrl=null, String Info=null)
   =>{
   Console.WriteLine($"content rootPath: {Environment.CurrentDirectory}");
   WebInfoContext wci = new();
   var userIpAddr = context.Connection.RemoteIpAddress;
   WebInfo wi = new (SiteDesc, $"{userIpAddr}", RefUrl, Info);
   wci.Add(wi);
   wci.SaveChanges();
   Console.WriteLine("Saved new data...");
});

app.MapGet("/Get/db", (HttpContext context, String pwd=null) =>{
      // post to this with /Get/db/?pwd=<your password>"
      WebInfoContext wci = new(); 
      var userIpAddr = context.Connection.RemoteIpAddress;
      // Saving off ip addr for attempt at this functionality 
      WebInfo wi = new ("get db", $"{userIpAddr}");
      wci.Add(wi);
      wci.SaveChanges();
      Console.WriteLine($"{userIpAddr}");
      Console.WriteLine($"{HelperTool.Hash(pwd)}");
      if (HelperTool.Hash(pwd) == "86BC2CA50432385C30E2FAC2923AA6D19F7304E213DAB1D967A8D063BEF50EE1"){

         var filePath = Path.Combine(Directory.GetCurrentDirectory(),  "logcap.db");
         if (!System.IO.File.Exists(filePath)) {return Results.NotFound();}
         return Results.File(filePath, "application/x-sqlite3", filePath);
      }
      else{
         return Results.NotFound();
      }
   });

app.MapGet("/GetDb", async context =>
{
      Console.WriteLine(Environment.CurrentDirectory);
    var html = await File.ReadAllTextAsync("getDb.htm");
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(html);
});

app.MapGet("/weatherforecast", (HttpContext context) =>
{
   WebInfoContext wci = new();
   var userIpAddr = context.Connection.RemoteIpAddress;
   WebInfo wi = new ("allos.dev", $"{userIpAddr}");
   wci.Add(wi);
   wci.SaveChanges();
   Console.WriteLine("Saved new data...");
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
