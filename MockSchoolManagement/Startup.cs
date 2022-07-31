using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MockSchoolManagement.CustomerMiddlewares;
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
        /// ����Ӧ�ó�������ķ���
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //��һ��·�����ò���
            //services.AddControllersWithViews(a => a.EnableEndpointRouting = false).AddXmlSerializerFormatters();

            //�ڶ���·�����ò���
            services.AddControllersWithViews(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));

            }).AddXmlSerializerFormatters();

            // ʹ��mvc�Դ���·�� EnableEndpointRouting false
            //services.AddMvc(a => a.EnableEndpointRouting = false);

            //services.AddSingleton<IStudentRepository, MockStudentRepository>();
            //services.AddTransient<IStudentRepository, MockStudentRepository>();
            //services.AddScoped<IStudentRepository, MockStudentRepository>();
            services.AddScoped<IStudentRepository, SqlStudentRepository>();

            //ʹ��sqlserver���ݿ⣬ͨ��IConfiguration����ȥ��ȡ���Զ������Ƶ�mock��StudentDBConnection��Ϊ�����ַ���
            services.AddDbContextPool<AppDbContext>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("MockStudentDbConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>().AddErrorDescriber<CustomIdentityErrorDescriber>().AddEntityFrameworkStores<AppDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;//������С������֤
                options.Password.RequireNonAlphanumeric = false;//����������һ������ĸ���ֵ��ַ�
                options.Password.RequiredUniqueChars = 3; //���������ظ��ַ���
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

            //��һ����������Ĭ�����õķ���(�Ƽ�ʹ��IdentityOptions��ʽ��ӦΪ��������Ϊһ�������ķ��������Ƕ����AddIdentity������)
            //services.AddIdentity<IdentityUser, IdentityRole>(options =>
            //{
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = false;
            //}).AddEntityFrameworkStores<AppDbContext>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// ����Ӧ�ó����������ܵ�
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //���м��ָ�����봥���쳣ʱ����뿪�����쳣ҳ��
                //������������ʹ�øý�������������գ�
                //1.�����ɹ�������ʹ�õ���ϸ��Ϣ 2.���쳣����������û�Ҳû���κ�����
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsStaging() || env.IsProduction() || env.IsEnvironment("UAT"))
            {
                app.UseExceptionHandler("/Error");

                //���ڴ�������쳣
                //app.UseStatusCodePages();

                //���������ռλ���������Զ�����Http�е�״̬�룬�������404���󣬻Ὣ�û��ض���/Error/404
                //app.UseStatusCodePagesWithRedirects("/Error/{0}");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");

            }
            app.UseStaticFiles();

            //app.UseMvcWithDefaultRoute();

            //�����֤�м��
            app.UseAuthentication();

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

            //         //��ֹ����
            //         context.Response.ContentType = "text/plain;charset=utf-8";
            //         //ע���ͨ��Configuration����
            //         await context.Response.Body.WriteAsync(System.Text.Encoding.Default.GetBytes(Configuration["MyKey"]));
            //     });
            //});

            app.UseAuthorization();

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
