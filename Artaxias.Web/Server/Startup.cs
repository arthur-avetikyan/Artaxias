using Artaxias.BusinessLogic;
using Artaxias.BusinessLogic.Account;
using Artaxias.BusinessLogic.Feedbacks;
using Artaxias.BusinessLogic.FileManagement;
using Artaxias.BusinessLogic.Notifications;
using Artaxias.BusinessLogic.Organization.Absences;
using Artaxias.BusinessLogic.Organization.Bonuses;
using Artaxias.BusinessLogic.Organization.Contracts;
using Artaxias.BusinessLogic.Organization.Departments;
using Artaxias.BusinessLogic.Organization.Employees;
using Artaxias.BusinessLogic.Organization.Salaries;
using Artaxias.BusinessLogic.Users;
using Artaxias.Core.Configurations;
using Artaxias.Core.Security.Authorization;
using Artaxias.Data;
using Artaxias.Data.Models;
using Artaxias.Data.Models.Feadback;
using Artaxias.Data.Models.Organization;
using Artaxias.Document.Generators;
using Artaxias.Document.Processors;
using Artaxias.Mailing;
using Artaxias.Models;
using Artaxias.Web.Server.Extensions;
using Artaxias.Web.Server.Filters;
using Artaxias.Web.Server.Handlers;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Reflection;
using System.Text;

namespace Artaxias.Web.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options => { options.Filters.Add(new ModelStateValidationFilter()); });
            services.AddRazorPages();
            services.Configure<ApplicationConfiguration>(Configuration.GetSection(nameof(ApplicationConfiguration)));
            services.Configure<MailingServiceConfiguration>(Configuration.GetSection(nameof(MailingServiceConfiguration)));
            string migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            void DbContextOptionsBuilder(DbContextOptionsBuilder builder)
            {
                builder.UseSqlServer(Configuration.GetConnectionString("ArtaxiasDatabaseConnection"));
            }

            services.AddDbContext<ArtaxiasDbContext>(DbContextOptionsBuilder);
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddIdentity<User, Role>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<ArtaxiasDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["ApplicationConfiguration:Authentication:Issuer"],
                        ValidAudience = Configuration["ApplicationConfiguration:Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["ApplicationConfiguration:Authentication:SecretKey"]))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddApplicationStaticPolicies();
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
            });

            services.AddSingleton<IAuthorizationPolicyProvider, PolicyProvider>();
            services.AddTransient<IAuthorizationHandler, PermissionRequirementHandler>();
            services.AddTransient<IAuthorizationHandler, SelfActionPermissionRequirementHandler>();

            services.AddScoped<IEmailProcessor, EmailProcessor>();
            services.AddSingleton<IFileManager, FileManager>();

            services.AddScoped<ITemplateProcessor, WordTemplateProcessor>();
            services.AddScoped<IDocumentGenerator, WordDocumentGenerator>();

            services.AddScoped<INotificationManager, NotificationManager>();

            services.AddScoped<IAuthenticationStateManager, AuthenticationStateManager>();
            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<IRoleManager, RoleManager>();
            services.AddScoped<IPermissionManager, PermissionManager>();
            services.AddScoped<IPermissionManager, PermissionManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IDepartmentManager, DepartmentManager>();
            services.AddScoped<IEmployeeManager, EmployeeManager>();
            services.AddScoped<ISalaryManager, SalaryManager>();
            services.AddScoped<ITemplateManager, TemplateManager>();
            services.AddScoped<IReviewManager, ReviewManager>();
            services.AddScoped<IAbsencesManager, AbsenceManager>();
            services.AddScoped<IFeedbackManager, FeedbackManager>();
            services.AddScoped<IBonusManager, BonusManager>();
            services.AddScoped<IContractManager, ContractManager>();

            services.AddScoped<IDomainStateVerifier<Absence>, AbsenceStateVerifier>();
            services.AddScoped<IDomainStateVerifier<Bonus>, BonusStateVerifier>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                //endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });

            InitializeApplicationDatabase(app);
        }

        private void InitializeApplicationDatabase(IApplicationBuilder app)
        {
            IServiceScopeFactory scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            IServiceScope scope = scopeFactory.CreateScope();
            RoleManager<Role> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            UserManager<User> userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            IRepository<Permission> permissionRepository = scope.ServiceProvider.GetRequiredService<IRepository<Permission>>();
            IRepository<RolePermission> rolePermissionRepository = scope.ServiceProvider.GetRequiredService<IRepository<RolePermission>>();
            IRepository<AbsenceType> absenceTypeRepository = scope.ServiceProvider.GetRequiredService<IRepository<AbsenceType>>();
            IRepository<DomainState> domainStateRepository = scope.ServiceProvider.GetRequiredService<IRepository<DomainState>>();
            IRepository<AnswerOptionType> answerOptionTypeRepository = scope.ServiceProvider.GetRequiredService<IRepository<AnswerOptionType>>();

            AppDatabaseInitializer appDatabaseInitializer =
                new AppDatabaseInitializer(userManager, roleManager, permissionRepository, rolePermissionRepository,
                                           absenceTypeRepository, domainStateRepository, answerOptionTypeRepository);
            appDatabaseInitializer.Initialize();
        }
    }
}
