var builder = WebApplication.CreateBuilder(args);

// configure baseURL for Dapr sidecar
var baseURL = (Environment.GetEnvironmentVariable("BASE_URL") ?? "http://localhost") + ":" + (Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3500"); //reconfigure code to make requests to Dapr sidecar

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient("catalog", (client) => {
    client.BaseAddress = new Uri(baseURL);
    client.DefaultRequestHeaders.Add("dapr-app-id", "catalog");  //enable Dapr service invoke
});
builder.Services.AddHttpClient("orders", (client) => {
    client.BaseAddress = new Uri(baseURL);
    client.DefaultRequestHeaders.Add("dapr-app-id", "orders");  //enable Dapr service invoke
});

var app = builder.Build();
app.Logger.LogInformation("Init: base URL set: " + baseURL);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
