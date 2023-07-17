using LearningSupportSystemAPI;
using LearningSupportSystemAPI.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "EduSystem API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


#region Connect to Azure App Configuration
// string azureAppConfigConnectionString = configuration.GetConnectionString("AppConfig")!;
string azureAppConfigConnectionString = "Endpoint=https://educonfig.azconfig.io;Id=cqJ8-lb-s0:Po2ljSSOYbBMttIz9P9j;Secret=ublhDyQZTRsUbQIwq1KEdL2+A3So3TjFCoJhNmK+AzE=";
configuration.AddAzureAppConfiguration(azureAppConfigConnectionString);
#endregion

#region [Register configurations]
// Register configs for using IOptionsMonitor<Config> in controllers and classes
//builder.Services.Configure<EduConfig>(configuration.GetSection(nameof(EduConfig)));
builder.Services.Configure<JwtTokenConfig>(configuration.GetSection(nameof(JwtTokenConfig)));
#endregion

#region [Connect to database]
var eduConfig = configuration.GetSection(nameof(EduConfig)).Get<EduConfig>();
builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
{
    options.UseSqlServer(eduConfig!.ConnectionString);
    // options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.EnableSensitiveDataLogging();
});
#endregion

#region [Add dependency injection]
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<ICoursePrerequisiteRepository, CoursePrerequisiteRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDiscussionRepository, DiscussionRepository>();
builder.Services.AddScoped<IGradeColumnRepository, GradeColumnRepository>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IMajorRepository, MajorRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
builder.Services.AddScoped<IToDoListRepository, ToDoListRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IFileSubmissionRepository, FileSubmissionRepository>();
builder.Services.AddScoped<IAnswerSubmissionRepository, AnswerSubmissionRepository>();

builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddScoped<IGenerateIdService, GenerateIdService>();
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
            .WithOrigins("http://localhost:3000", "https://edu-assistant.netlify.app")
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

builder.Services.AddIdentityCore<Student>()
    .AddRoles<Role>()
    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Student, Role>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserManager<StudentManager>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityCore<Lecturer>()
    .AddRoles<Role>()
    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Lecturer, Role>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserManager<LecturerManager>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityCore<Administrator>()
    .AddRoles<Role>()
    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Administrator, Role>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserManager<AdminManager>()
    .AddDefaultTokenProviders();
#endregion

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LearningSupportSystem.Api v1"));

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("ClientPermission"); // Use the CORS policy

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<DiscussionHub>("/hub");

app.Run();
