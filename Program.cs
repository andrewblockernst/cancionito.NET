// Import the required packages
//==============================
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;

// Set your Cloudinary credentials
//=================================
DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
var cloudinaryUrl = Environment.GetEnvironmentVariable("CLOUDINARY_URL");

if (string.IsNullOrEmpty(cloudinaryUrl))
{
    throw new Exception("La variable de entorno 'CLOUDINARY_URL' no está configurada.");
}

Cloudinary cloudinary = new Cloudinary(cloudinaryUrl);
cloudinary.Api.Secure = true;

var builder = WebApplication.CreateBuilder(args);

// Inyección de dependencias
builder.Services.AddSingleton(cloudinary); // Inyecta la instancia de Cloudinary
builder.Services.AddScoped<ICloudinaryPeronService, CloudinaryPeronService>(); // Inyecta el servicio CloudinaryService

//===========================
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlite<CancionitoContext>(builder.Configuration.GetConnectionString("cnCancionito"));
builder.Services.AddScoped<ISongService, SongDbService>();
builder.Services.AddScoped<IImageService, ImageDbService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();