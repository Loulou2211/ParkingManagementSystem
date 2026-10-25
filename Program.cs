using ParkingManagementSystem.Services;
using ParkingManagementSystem;
using ParkingManagementSystem.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DatabaseConnection>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<VehicleService>();
builder.Services.AddScoped<ParkingSpaceService>();
builder.Services.AddScoped<ParkingSessionService>();
builder.Services.AddScoped<StaffService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();
try
{
    using var connection = new DatabaseConnection(
        app.Services.GetRequiredService<IConfiguration>()
    ).GetConnection();

    await connection.OpenAsync();
    Console.WriteLine("MySQL connection successful!");
}
catch (Exception ex)
{
    Console.WriteLine($"MySQL connection failed: {ex.Message}");
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();