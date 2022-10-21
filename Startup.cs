using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SkillBoxAPI.Context;
using SkillBoxAPI.Data;
using SkillBoxAPI.Interfaces;
using SkillBoxAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkillBoxAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private string path;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ReqDbContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IReq, ReqData>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = AuthOptions.ISSUER,
                            ValidateAudience = true,
                            ValidAudience = AuthOptions.AUDIENCE,
                            ValidateLifetime = true,
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                    });

            services.AddCors();


            services.AddControllers();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {

            app.UseRouting();
            app.UseCors(builder => builder.WithOrigins("http://localhost:19378").AllowAnyHeader()
                            .AllowAnyMethod().AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            path = env.ContentRootPath;
            CreateStartItems(serviceProvider);
        }

        private void CreateStartItems(IServiceProvider serviceProvider)
        {
            try
            {
                ReqDbContext context = serviceProvider.GetRequiredService<ReqDbContext>();
                var testUser = context.Users.FirstOrDefault(x => x.Username == "admin");
                if (testUser == null)
                {
                    var add = context.Users.Add(new User() { Username = "Admin", Password = "Admin", Role = "Admin" });
                    context.SaveChanges();
                }

                //Создаем начальные записи в блог, проекты и услуги
                var items = context.Htmls.ToList();
                if (items.Count == 0)
                {
                    var jsonList = File.ReadAllLines($"{path}/json.txt");

                    List<HtmlTemplate> list = new List<HtmlTemplate>();

                    foreach (var item in jsonList)
                        list.Add(JsonConvert.DeserializeObject<HtmlTemplate>(item));

                    foreach (var item in list)
                    {
                        var add = context.Htmls.Add(item);
                        var save = context.SaveChanges();
                    }
                }

                //Создаем начальные заявки
                var reqs = context.Reqs.ToList();
                if(reqs.Count == 0)
                {
                    var names = File.ReadAllLines($"{path}/names.txt").ToList();
                    Random r = new Random(Guid.NewGuid().GetHashCode());

                    for (int i = 0; i < 50; i++)
                    {
                        string name = names[r.Next(names.Count)];
                        string[] statuses = new string[] { "Получена", "В работе", "Выполнена", "Отклонена", "Отменена" };
                        var add = context.Reqs.Add(new Req()
                        {
                            Email = $"{name}{r.Next(100, 9999)}@gmail.com",
                            Name = name,
                            Status = statuses[r.Next(statuses.Length)],
                            Text = $"Тестовая заявка {r.Next(100, 9999)}",
                            Time = new DateTime(2022, r.Next(7,11) , r.Next(1, DateTime.Now.Day), r.Next(1, 24), r.Next(1, 59), r.Next(1, 59))
                        });
                        var save = context.SaveChanges();
                    }
                }

            }
            catch (Exception exc)
            {
                new Exception(exc.ToString());
            }
        }
    }
}
