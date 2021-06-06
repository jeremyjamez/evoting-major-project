using eVotingApi.Config;
using eVotingApi.Data;
using eVotingApi.Models;
using eVotingApi.Models.Auth;
using eVotingApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eVotingApi
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
            
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

            services.Configure<EVotingDatabaseSettings>(
                Configuration.GetSection(nameof(EVotingDatabaseSettings)));

            services.AddSingleton<IEVotingDatabaseSettings>(sp =>
            sp.GetRequiredService<IOptions<EVotingDatabaseSettings>>().Value);

            services.AddSingleton<VoterService>();
            services.AddSingleton<ConstituencyService>();
            services.AddSingleton<CandidateService>();
            services.AddSingleton<PartyService>();
            services.AddSingleton<ElectionService>();

            services.AddDbContext<eVotingContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("eVotingDatabase"))
            );

            services.AddSpaStaticFiles(configuration: options => { options.RootPath = "wwwroot"; });

            var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(tokenValidationParams);

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt => {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenValidationParams;
            });

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<eVotingContext>();

            services.AddCors(options =>
            {
                options.AddPolicy("NextJsCorsPolicy", builder =>
                {
                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:3000");
                });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("NextJsCorsPolicy");

            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
                RequestPath = new PathString("/wwwroot")
            });

            app.UseAuthentication();

            MyIdentityDataInitializer.SeedData(userManager, roleManager);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class MyIdentityDataInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new IdentityRole();
                role.Name = "Administrator";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("EDW").Result)
            {
                var role = new IdentityRole();
                role.Name = "EDW";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("EOJ").Result)
            {
                var role = new IdentityRole()
                {
                    Name = "EOJ"
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
