using System.Text;
using System.Threading.Tasks;
using AppTest.Db;
using AppTest.Helpers;
using AppTest.Queries;
using AppTest.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AppTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("mainDb");

            services.AddEntityFrameworkNpgsql()
                    .AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString))
                    .AddLogging();

            services.AddIdentity<IdentityUser, IdentityRole>(cfg => { cfg.User.RequireUniqueEmail = true; })
                .AddEntityFrameworkStores<ApplicationContext>();

            var appSettingsSection = Configuration.GetSection("auth:jwt");
            services.Configure<AppSettings>(appSettingsSection);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    buider =>
                    {
                        buider.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(option =>
                    {
                        option.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["auth:jwt:Secret"]))
                        };

                        option.RequireHttpsMetadata = false;
                        option.SaveToken = true;
                        option.Events = new JwtBearerEvents
                        {
                            OnTokenValidated = context =>
                            {
                                var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                                var userId = int.Parse(context.Principal.Identity.Name);
                                var user = userService.GetById(userId);
                                if (user == null) context.Fail("Unauthorized");

                                return Task.CompletedTask;
                            }

                        };
                    });

            services.AddMvc(mvc => { mvc.EnableEndpointRouting = false; });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });

            services.AddScoped<IUserService, UserService>();
            services.AddTransient<UserQuery>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseAuthentication();
            app.UseMvc();
            app.UsePathBase("/");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
