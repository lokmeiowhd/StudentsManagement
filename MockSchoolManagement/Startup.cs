using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MockSchoolManagement.DataRepositories;
using MockSchoolManagement.Infrastructure;

namespace MockSchoolManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 配置应用程序所需的服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(a => a.EnableEndpointRouting = false).AddXmlSerializerFormatters();

            // 使用mvc自带的路由 EnableEndpointRouting false
            //services.AddMvc(a => a.EnableEndpointRouting = false);

            //services.AddSingleton<IStudentRepository, MockStudentRepository>();
            //services.AddTransient<IStudentRepository, MockStudentRepository>();
            //services.AddScoped<IStudentRepository, MockStudentRepository>();
            services.AddScoped<IStudentRepository, SqlStudentRepository>();

            //使用sqlserver数据库，通过IConfiguration访问去获取，自定义名称的mock，StudentDBConnection作为连接字符串
            services.AddDbContextPool<AppDbContext>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("MockStudentDbConnection")));



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 配置应用程序的请求处理管道
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //该中间件指当代码触发异常时会进入开发者异常页面
                //在生产环境中使用该界面存在两个风险：
                //1.包含可供攻击者使用的详细信息 2.该异常界面对最终用户也没有任何意义
                app.UseDeveloperExceptionPage();
            }
            else if(env.IsStaging()|| env.IsProduction()||env.IsEnvironment("UAT"))
            {
                app.UseExceptionHandler("/Error");

                //用于处理错误异常
                //app.UseStatusCodePages();

                //这里采用了占位符，它会自动接收Http中的状态码，如果出现404错误，会将用户重定向到/Error/404
                //app.UseStatusCodePagesWithRedirects("/Error/{0}");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");

            }
            app.UseStaticFiles();

            //app.UseMvcWithDefaultRoute();

            app.UseRouting();

            //app.UseAuthorization();

            //app.Run(async (context) =>
            //{
            //    context.Response.ContentType = "text/plain;charset=utf-8";
            //    await context.Response.Body.WriteAsync(System.Text.Encoding.Default.GetBytes("Hello World!"));
            //});

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //    endpoints.MapGet("/", async context =>
            //     {
            //         //var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            //         //await context.Response.Body.WriteAsync(System.Text.Encoding.Default.GetBytes(processName));

            //         //防止乱码
            //         context.Response.ContentType = "text/plain;charset=utf-8";
            //         //注入后通过Configuration访问
            //         await context.Response.Body.WriteAsync(System.Text.Encoding.Default.GetBytes(Configuration["MyKey"]));
            //     });
            //});
            app.UseEndpoints(endPoints =>
            {
                endPoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
