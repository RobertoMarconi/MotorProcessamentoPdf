using API.Shared.Filters;
using AutoMapper;
using Business.Core;
using Business.Core.ICore;
using Business.Helpers.AssinaturaDigital;
using Business.Shared.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prodest.HealthCheck;

namespace API
{
    public class Startup
    {
        private readonly IHostEnvironment _env;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;

            _env = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICarimboCore, CarimboCore>();
            services.AddTransient<ITransformaPdfCore, TransformaPdfCore>();
            services.AddTransient<IAssinaturaDigitalCore, AssinaturaDigitalCore>();
            services.AddControllers(options =>
                options.Filters.Add(new HttpResponseExceptionFilter())
            );

            services.AddSwaggerGen();

            services.AddCustomHealthChecks(new CustomHealthCheckOptions
            {
                ContentRootFileProvider = _env.ContentRootFileProvider
            });

            // configurando automapper
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CertificadoDigital, CertificadoDigitalDto>();
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            //services.Configure<KestrelServerOptions>(options =>
            //{
            //    options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
            //});

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Motor de Processamento de PDF - V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCustomHealthChecks();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
