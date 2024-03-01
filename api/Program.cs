using mvc.Repositories;
using Microsoft.Extensions.Caching.StackExchangeRedis;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IEmployeeRepository,EmployeeRepository>();
builder.Services.AddSingleton<IUserRepositories,UserRepositories>();
builder.Services.AddHttpContextAccessor(); // Add this line to register HttpContextAccessor

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
// builder.Services.AddSession(options =>
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "Master";
});

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "project";
    // Add other session options as needed
});


var app = builder.Build();
app.UseCors("corsapp"); 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
