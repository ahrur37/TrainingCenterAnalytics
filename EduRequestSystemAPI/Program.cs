using EduRequestSystemAPI.DatabaseContext;
using EduRequestSystemAPI.Hubs;
using EduRequestSystemAPI.Interfaces;
using EduRequestSystemAPI.Services;
using EduRequestSystemAPI.UniversalMethods;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using EduRequestSystemAPI.Services.Implementations;
using Microsoft.AspNetCore.Authorization.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddDbContext<ContextDb>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("TestDbString")), ServiceLifetime.Scoped);

builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IDictionariesService, DictionariesService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddSingleton<jwtGenerator>();

builder.Services.AddSignalR();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContextDb>();
    db.Database.Migrate();

    var seedPath = Path.Combine(AppContext.BaseDirectory, "seed.sql");
    if (File.Exists(seedPath) && !db.Roles.Any())
    {
        var sql = File.ReadAllText(seedPath);
        db.Database.ExecuteSqlRaw(sql);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<CommentHub>("/hubs/comments");

app.Run();
