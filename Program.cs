using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SkillBoxAPI.Context;
using SkillBoxAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillBoxAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var init = BuildWebHost(args);

            using (var scope = init.Services.CreateScope())
            {
                var s = scope.ServiceProvider;
                var c = s.GetRequiredService<ReqDbContext>();
                DbInitializer.Initialize(c);
            }

            init.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
