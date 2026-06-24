using UserVaultApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI (lets you explore + test endpoints at /swagger too, alongside Postman)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "UserVaultApi",
        Version = "v1",
        Description = "A simple CRUD API for managing users. Test it with Postman or Swagger."
    });
});

// In-memory "vault" store, shared across requests for the app's lifetime
builder.Services.AddSingleton<UserStore>();

// CORS - open for local testing (tighten this for real deployments)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserVaultApi v1");
});

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
