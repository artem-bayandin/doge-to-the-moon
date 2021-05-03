using DogeData.Context;
using DogeData.Repos;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Repos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            // });

            services.AddScoped<IDogeDbContext, DogeDbContext>();
            services.AddScoped<ITransactionRepo, TransactionRepo>();

            services
                .AddDbContext<DogeDbContext>(
                    options =>
                        options.UseSqlServer(
                            Configuration.GetConnectionString("DogeDbContext"),
                            // enable auto migrations
                            optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(DogeDbContext).GetTypeInfo().Assembly.GetName().Name)
                        )
            );

            services.AddMediatR(typeof(Domain.Interfaces.IDogeDbContext));

            services.AddIdentity<UserEntity, UserEntityRole>()
                .AddEntityFrameworkStores<DogeDbContext>()
                .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseSwagger();
                // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
