using System.Text.Json.Serialization;
using Notion.Application.Extensions;
using Notion.Domain.DI;
using Notion.WebApi.Extensions;
using Notion.WebApi.Middlewares;

namespace Notion.WebApi
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            services
                .AddMongoConnection(Configuration)
                .AddOptions(Configuration)
                .AddRepository()
                .AddServices()
                .AddMapper();
            
            services.ConfigureBadRequestResponse();
            
            services.AddEndpointsApiExplorer();
            
            services.AddSwaggerGen();  
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "SoundShare.Notion");
                });
            }

            Exceptions.UseExceptionHandler(app);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}