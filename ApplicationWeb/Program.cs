using ApplicationWeb;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var connectionString = builder.Configuration.GetValue<String>("ConnectionStrings:DefaultConnection");

DependencyResolver.Resolver(builder.Services);

var app = builder.Build();

DependencyResolver.ConfigureContext(app.Services, connectionString);

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.ConfigureExceptionHandler();

app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.MapControllers();

app.Run();
