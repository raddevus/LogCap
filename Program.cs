using LogCap.Model;

var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/Save/{RefUrl}/{Info}", async(String RefUrl, String Info)
      =>{
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
