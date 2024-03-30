using library_management.Data;
using library_management.Data.Model;
using library_management.Data.Services;
using library_management.Data.ViewModel.Email;
using library_management.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.AddTransient<IAuthRepository, AuthRepository>();
builder.Services.AddTransient<IEmailServices, EmailServices>();

//configure services with SMTPConfiguration
builder.Services.Configure<SMTPConfiguration>(builder.Configuration.GetSection("EmailSettings"));

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{

    var servicesProvider = scope.ServiceProvider;
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    try
    {
        AppDbInitializer.InitializerAsync(servicesProvider, userManager).Wait();
    }
    catch (Exception ex)
    {
        var logger = servicesProvider.GetRequiredService<Logger<Program>>();
        logger.LogError(ex, "Error in Program file.");
    }
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
