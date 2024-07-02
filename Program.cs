using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PracticeForRevision.DAL;
using PracticeForRevision.Infrastructure.Interface;
using PracticeForRevision.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddHttpClient<IPaymentService, PaymentService>();
builder.Services.AddTransient<IExportService, ExportService>();
builder.Services.AddTransient<ILoggingService, LoggingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var loggingService = context.RequestServices.GetRequiredService<ILoggingService>();

            if (exceptionHandlerPathFeature?.Error != null)
            {
                await loggingService.LogErrorAsync(exceptionHandlerPathFeature.Error, exceptionHandlerPathFeature.Path);
            }

            context.Response.StatusCode = 500; // Set appropriate status code
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("An unexpected fault happened. Please try again later.");
        });
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

// Logging middleware to log actions
app.Use(async (context, next) =>
{
    var loggingService = context.RequestServices.GetRequiredService<ILoggingService>();
    var actionName = context.Request.Path;

    try
    {
        await next();
        await loggingService.LogActionAsync(actionName, "Action executed successfully");
    }
    catch (Exception ex)
    {
        await loggingService.LogErrorAsync(ex, actionName);
        throw; // Re-throw the exception to preserve the original behavior
    }
});

app.Run();
