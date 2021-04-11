using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using DAL;

namespace Main
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<AllDbContext>();
            services.AddTransient<DbContextLoader>();
            services.AddTransient<LoginService>();
            services.AddTransient<MapperService>();
            services.AddSingleton<RegisterService>();
            services.AddSingleton<ClientPipeHanlder>();
            services.AddTransient<Services.UpdateHandlerService>();
            services.AddSingleton<BL.OrderService>();
            services.AddSingleton<BL.ToursService>();
            services.AddSingleton<BL.SourceService>();
            services.AddSingleton<UserService>();
        }
    }
}
