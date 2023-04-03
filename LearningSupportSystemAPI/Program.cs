using LearningSupportSystemAPI.Contract;
using LearningSupportSystemAPI.Core.Database;
using LearningSupportSystemAPI.Core.Entities;
using LearningSupportSystemAPI.Repository;
using LearningSupportSystemAPI.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Connect to Azure App Configuration
string azureAppConfigConnectionString = configuration.GetConnectionString("AppConfig")!;
configuration.AddAzureAppConfiguration(azureAppConfigConnectionString);
#endregion

#region [Register configurations]
// Register configs for using IOptionsMonitor<Config> in controllers and classes
//builder.Services.Configure<EduConfig>(configuration.GetSection(nameof(EduConfig)));
builder.Services.Configure<JwtTokenConfig>(configuration.GetSection(nameof(JwtTokenConfig)));
#endregion

#region [Connect to database]
var eduConfig = configuration.GetSection(nameof(EduConfig)).Get<EduConfig>();
builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(eduConfig!.ConnectionString));
#endregion

#region [Add dependency injection]
builder.Services.AddTransient<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddTransient<IClassRepository, ClassRepository>();
builder.Services.AddTransient<ICoursePrerequisiteRepository, CoursePrerequisiteRepository>();
builder.Services.AddTransient<ICourseRepository, CourseRepository>();
builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddTransient<IDiscussionRepository, DiscussionRepository>();
builder.Services.AddTransient<IGradeColumnRepository, GradeColumnRepository>();
builder.Services.AddTransient<IGradeRepository, GradeRepository>();
builder.Services.AddTransient<IGroupRepository, GroupRepository>();
builder.Services.AddTransient<IMajorRepository, MajorRepository>();
builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<INotificationRepository, NotificationRepository>();
builder.Services.AddTransient<IRoomRepository, RoomRepository>();
builder.Services.AddTransient<ISemesterRepository, SemesterRepository>();
#endregion

#region [Add Authentication]
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = false;

    var jwtConfig = configuration.GetSection(nameof(JwtTokenConfig)).Get<JwtTokenConfig>();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig!.Key)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});
#endregion

#region [Add Automapper]
builder.Services.AddAutoMapper(typeof(Program));
#endregion

#region [Add CORS]
builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPermission", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000", "https://blog-hub.netlify.app")
            .AllowCredentials();
    });
});
#endregion

#region [Add Identity]
builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 1;

    options.User.RequireUniqueEmail = false;
    options.SignIn.RequireConfirmedEmail = false;
})
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<UserManager>()
                .AddDefaultTokenProviders();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("ClientPermission"); // Use the CORS policy

app.UseAuthorization();

app.MapControllers();

app.Run();
