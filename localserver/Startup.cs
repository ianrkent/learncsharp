using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace localserver
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;
        public readonly IConfiguration _config;

        public Startup(ILogger<Startup> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        
        public void ConfigureServices(IServiceCollection services) { 

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            foreach (var spec in _config.GetSection("staticContent").GetChildren())
            {
                var staticContentOptions = new StaticContentOptions();
                spec.Bind(staticContentOptions);

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(staticContentOptions.LocalFilePath),
                    RequestPath = $"/{ staticContentOptions.UrlRoutePath }"
                });

                _logger.LogInformation($"Static content in folder { staticContentOptions.LocalFilePath } is available on path /{ staticContentOptions.UrlRoutePath }");
            }
        }
    }
}
