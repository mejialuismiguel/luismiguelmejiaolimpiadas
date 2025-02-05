using Serilog;
using Serilog.Sinks.MSSqlServer;
using Microsoft.EntityFrameworkCore;

using AthleteApi.Services;
using AthleteApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Obtener las banderas de configuración para logs
// en caso de querer administrar independientemente sin escoger la una o la otra agregar la banderas al appsettings.json:
//      "EnableFileLogging": true,
//      "EnableDbLogging": false,
// con estas banderas se puede habilitar o deshabilitar el log en archivo o en base de datos.
// Banderas o switches para habilitar o deshabilitar el log en archivo o en base de datos
var enableFileLogging = false;
var enableDbLogging = false;
// Se debe colocar en el appsettings.json File para guardar en el archivo log o DB para que guarde en base de datos.
var SwitchLogDBorFile = builder.Configuration.GetValue<string>("Serilog:SwitchLogDBorFile"); 
// "file" para archivo, "db" para base de datos en caso de no existir o datos incorrectos el defecto sera file
if (SwitchLogDBorFile == "file")
{
    enableFileLogging = true;
}
else if (SwitchLogDBorFile == "db")
{
    enableDbLogging = true;
}
else
{
    enableFileLogging = true;
}
// enableFileLogging = builder.Configuration.GetValue<bool>("Serilog:EnableFileLogging"); // Descomentar para habilitacion independiente de log
// enableDbLogging = builder.Configuration.GetValue<bool>("Serilog:EnableDbLogging"); // Descomentar para habilitacion independiente de log

var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console();

// Agregar el log de archivo solo si está habilitado
if (enableFileLogging)
{
    var filePath = builder.Configuration["Serilog:WriteTo:0:Args:path"];
    if (!string.IsNullOrEmpty(filePath))
    {
        loggerConfig.WriteTo.File(
            path: filePath,
            rollingInterval: RollingInterval.Day,
            outputTemplate: builder.Configuration["Serilog:WriteTo:0:Args:outputTemplate"] ?? "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
        );
    }
}

// Agregar el log de base de datos solo si está habilitado
if (enableDbLogging)
{
    var connectionString = builder.Configuration["Serilog:WriteTo:1:Args:connectionString"]; // Obtener la conexión desde JSON
    if (!string.IsNullOrEmpty(connectionString))
    {
        var sinkOptions = new MSSqlServerSinkOptions
        {
            TableName = builder.Configuration["Serilog:WriteTo:1:Args:tableName"] ?? "Logs",
            AutoCreateSqlTable = true
        };
        loggerConfig.WriteTo.MSSqlServer(
            connectionString: connectionString,
            sinkOptions: sinkOptions
        );
    }
}

Log.Logger = loggerConfig.CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AthleteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAthleteService, AthleteService>();
builder.Services.AddScoped<ITournamentService, TournamentService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IWeightCategoryService, WeightCategoryService>();
builder.Services.AddScoped<ITournamentParticipationService, TournamentParticipationService>();
builder.Services.AddScoped<IAttemptService, AttemptService>();
builder.Services.AddScoped<IAthleteAttemptSummaryService, AthleteAttemptSummaryService>();
builder.Services.AddScoped<ICompetitionResultService, CompetitionResultService>();

// Register the Swagger generator, defining one or more Swagger documents
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Athlete API",
        Description = "Terminos y Condiciones",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Athletes",
            Email = string.Empty,
            Url = new Uri("https://twitter.com/yourname"),
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "Use under LICX",
            Url = new Uri("https://example.com/license"),
        }
    });

    // Add JWT Authentication to Swagger
    var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
        Reference = new Microsoft.OpenApi.Models.OpenApiReference
        {
            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { securityScheme, new[] { "Bearer" } }
    };

    c.AddSecurityRequirement(securityRequirement);
});

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "JWT no esta configurada la llave.");
var key = System.Text.Encoding.ASCII.GetBytes(jwtKey);
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Use Serilog request logging
app.UseSerilogRequestLogging();

// Map controllers directly
app.MapControllers();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Athlete API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

app.Run();