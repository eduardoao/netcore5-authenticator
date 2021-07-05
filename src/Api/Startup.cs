using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;

namespace WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // adiciona os serviços para o DI container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>();
            services.AddCors();
            services.AddControllers().AddJsonOptions(json => json.JsonSerializerOptions.IgnoreNullValues = true);

            // definir objeto de configurações fortemente tipado
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // configurar DI para serviços de aplicativo
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IUserService, UserService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Authenticator JWT -NetCore 5.0 2021", Version = "v1" });
            });
        }

        // configurar o pipeline de solicitação HTTP
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authenticator JWT -NetCore 5.0 2021"));
            }


            CreateTestUser(context);

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            // manipulador de erro global
            app.UseMiddleware<ErrorHandlerMiddleware>();

            // middleware jwt auth personalizado
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoint => endpoint.MapControllers());
        }

        private void CreateTestUser(DataContext context)
        {
            // adicionar usuário de teste codificado ao banco de dados na inicialização
            var mockUser = new User
            {
                FirstName = "Eduardo",
                LastName = "Alcantara de Oliveira",
                Username = "Admin",
                PasswordHash = BCryptNet.HashPassword("admin")
            };
            context.Users.Add(mockUser);
            context.SaveChanges();
        }
    }
}
