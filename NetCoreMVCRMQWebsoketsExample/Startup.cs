using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreMVCRMQWebsoketsExample.Hubs;
using NetCoreMVCRMQWebsoketsExample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreMVCRMQWebsoketsExample
{
    public class Startup
    {
        private IConfiguration _configuration { get; set; }

        public Startup(IConfiguration config)
        {

            _configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {             
            services.AddMvc();
            services.AddSignalR();
            services.AddSingleton<IMessageQueueClient, MessageQueueClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}"
                   );
                endpoints.MapHub<MessageHub>("/messageHub");
            });
        }
    }
}
