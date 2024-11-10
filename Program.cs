// Import the required packages
//=====================================================================================//
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

//=====================================================================================//

//Load the environment variables from the .env file
DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));

//Retrieve the Cloudinary URL from the environment variables
var cloudinaryUrl = Environment.GetEnvironmentVariable("CLOUDINARY_URL");

//Throw an exception if the Cloudinary URL is not set
if (string.IsNullOrEmpty(cloudinaryUrl)) {
    throw new Exception("The 'CLOUDINARY_URL' environment variable is not set.");
}

//Initialize the Cloudinary instance with the Cloudinary URL
Cloudinary cloudinary = new Cloudinary(cloudinaryUrl);
cloudinary.Api.Secure = true;

//Set up the web application builder
var builder = WebApplication.CreateBuilder(args);

//Register Cloudinary as a service in the application container 
builder.Services.AddSingleton(cloudinary); 

//Register the Cloudinary 'service' in the application container
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>(); 

//=====================================================================================//

//Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    //Basic configuration of Swagger
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cancionito", Version = "v1" });

    //Add a security definition for JWT Bearer tokens in the Swagger 
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the JWT token in this format: Bearer {token}"
    });

    //Enforce security requirements for JWT Bearer tokens in the Swagger for all endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

//=====================================================================================//

//Register SQLite as the database provider for the CancionitoContext
builder.Services.AddSqlite<CancionitoContext>(builder.Configuration.GetConnectionString("DefaultConnection"));

//Register the CancionitoContext as a service in the application container (Songs and Images)
builder.Services.AddScoped<ISongService, SongDbService>();
builder.Services.AddScoped<IImageService, ImageDbService>();

//======================= AUTHENTICATION AND AUTHORIZATION ===========================//

//Configure Identity and JWT for authentication and authorization
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("cnCancionito")));

//Add Identity services to the container
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//Configure JWT authentication
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, //Ensure that the token was issued by a trusted authorization server
        ValidateAudience = true, //Ensure that the token is intended for the audience
        ValidateLifetime = true, //Check if the token is not expired
        ValidateIssuerSigningKey = true, //Verify that the key used to sign the incoming token is part of a list of trusted keys
        ValidIssuer = builder.Configuration["Jwt:Issuer"], //Expected issuer of the token
        ValidAudience = builder.Configuration["Jwt:Audience"], //Expected audience of the token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) //Key used to sign the incoming token
    };
});

//=====================================================================================//


//Build the application
var app = builder.Build();

//Apply pending migrations to the database
using (var scope = app.Services.CreateScope()) {
    var dbContext = scope.ServiceProvider.GetRequiredService<CancionitoContext>();
    dbContext.Database.Migrate(); // Ejecuta las migraciones pendientes
}

// -----------------------------------------------------------------------------------//

// WE HAVE THIS SECTION OF CODE TO MANAGE THOSE MIGRATIONS THAT ARE PENDING 
// (IN CASE WE NEED TO UPDATE THE DATABASE from ApplicationDbContext)

// using (var scope = app.Services.CreateScope()) {
//     var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     dbContext.Database.Migrate(); // Ejecuta las migraciones pendientes
// }

// -----------------------------------------------------------------------------------//

//Swagger middleware configuration for API documentation and testing
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Enable HTTPS redirection and map controllers
app.UseHttpsRedirection();
app.MapControllers();

//Run the application
app.Run();