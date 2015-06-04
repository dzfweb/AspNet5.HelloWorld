using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json.Serialization;

namespace HelloWeb
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();            
        }
        
        public void ConfigureServices(IServiceCollection services)
        { 
            services.AddMvc().Configure<MvcOptions>(options =>
            {
                options.InputFormatters.Clear();
             
                var jsonOutputFormatter = new JsonOutputFormatter();
                jsonOutputFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                jsonOutputFormatter.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
                jsonOutputFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                                
                options.OutputFormatters.Insert(0, jsonOutputFormatter);
            });  
        }
    }
}