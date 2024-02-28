using mvc.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddSingleton<IEmployeeRepository,EmployeeRepository>();
builder.Services.AddSingleton<IUserRepositories,UserRepositories>();
builder.Services.AddHttpContextAccessor(); // Add this line to register HttpContextAccessor

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
// builder.Services.AddSession(options =>
// {
//     // Configure session options as needed
// });
builder.Services.AddDistributedMemoryCache(); 
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UseCors("corsapp"); 
// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    });
}
app.UseHttpsRedirection();
// app.UseSession();

app.UseSession();
app.UseAuthorization();

app.MapControllers();

app.Run();
