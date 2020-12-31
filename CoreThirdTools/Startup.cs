using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreThirdTools.Helper;
using CoreThirdTools.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CoreThirdTools
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<Captcha>();
            services.AddSingleton<Email>();
            services.AddSingleton<QRCode>();
            //services.AddSingleton(new Appsettings(Configuration));

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "接口文档",
                    Description = "RESTful API for Hicard",
                });
            });
            #endregion

            #region Jwt
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证颁发者
                        ValidateAudience = true,//是否验证访问群体
                        ValidateLifetime = true,//是否验证生存期
                        ClockSkew = TimeSpan.FromSeconds(Convert.ToInt32(AppSettings.JWT.ClockSkew)),//验证Token的时间偏移量
                        ValidateIssuerSigningKey = true,//是否验证安全密钥
                        ValidAudience = AppSettings.JWT.ValidAudience,//访问群体
                        ValidIssuer = AppSettings.JWT.ValidIssuer,//颁发者
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.JWT.SecurityKey))//安全密钥
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();

                            context.Response.ContentType = "application/json;charset=utf-8";
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                            await context.Response.WriteAsync("{\"message\":\"Unauthorized\",\"success\":false}");
                        },
                        //将token放在URL参数中
                        //OnMessageReceived = async context =>
                        //{
                        //    context.Token = context.Request.Query["token"];

                        //    await Task.CompletedTask;
                        //}
                    };
                });

            services.AddAuthorization();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
                RequestPath = "/StaticFiles"
            });

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Demo v1");
            });
            #endregion

            app.UseRouting();

            #region Jwt
            // 身份验证
            app.UseAuthentication();
            // 认证授权
            app.UseAuthorization();
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
