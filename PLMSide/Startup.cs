using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using PLMSide.Data;
using PLMSide.Filter;
using PLMSide.Log;
using Microsoft.AspNetCore.Mvc.Formatters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using PLMSide.Validator;

namespace PLMSide
{
    public class Startup
    {
        /// <summary>
        /// log4net 仓储库
        /// </summary>
        public static ILoggerRepository repository { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            repository = LogManager.CreateRepository("PLMSide");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        //修改返回值为IServiceProvider，使用Autofac依赖注入容器
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var audienceConfig = Configuration.GetSection("Audience");

            services.AddSingleton<IMemoryCache>(factory =>
            {
                var cache = new MemoryCache(new MemoryCacheOptions());
                return cache;
            });

            //services.AddSingleton<DataBaseConfig>(fac=>
            //{
            //    return new Configuration();
            //});

            #region Option
            services.AddOptions();
            services.Configure<SqlOptions>(Configuration.GetSection("DataConfig"));
            services.AddScoped<PLMSide.Data.DataBaseConfig>();

            #endregion

            #region LOG注入
            services.AddSingleton<ILoggerHelper, LogHelper>();
            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new Info
                {
                    Version = "v0.1.0",
                    Title = "Web API",
                    Description = "框架说明文档",
                    TermsOfService = "None",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact { Name = "PLMSide", Email = "Web.Core@xxx.com", Url = "#" }
                });

                #region Token绑定到ConfigureServices
                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "PLMSide", new string[] { } }, };
                c.AddSecurityRequirement(security);
                //方案名称“Blog.Core”可自定义，上下一致即可
                c.AddSecurityDefinition("PLMSide", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
                #endregion
            });

            #endregion

            #region JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = audienceConfig["Audience"],//Audience
                        ValidIssuer = audienceConfig["Issuer"],//Issuer，这两项和前面签发jwt的设置一致
                        ClockSkew = TimeSpan.FromMinutes(30),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(audienceConfig["Secret"]))//拿到SecurityKey
                    };
                });


            #endregion

            #region CORS
            services.AddCors(c =>
            {
                c.AddPolicy("LimitRequests", policy =>
                {
                    //policy
                    //.WithOrigins(Configuration["RequireHost"])//支持多个域名端口，注意端口号后不要带/斜杆
                    //.AllowAnyHeader()//Ensures that the policy allows any header.
                    //.AllowAnyMethod();
                    policy
                   .AllowAnyOrigin()//允许任何源
                   .AllowAnyMethod()//允许任何方式
                   .AllowAnyHeader()//允许任何头
                   .AllowCredentials();//允许cookie
                });
            });

            #endregion

            services.AddMvc(o =>
            {
                // 全局异常过滤
                o.Filters.Add(typeof(GlobalExceptionsFilter));
                o.OutputFormatters.RemoveType(typeof(HttpNoContentOutputFormatter));
                o.OutputFormatters.Insert(0, new HttpNoContentOutputFormatter { TreatNullValueAsNoContent=false });
            }
            )
            .AddFluentValidation(
                 o =>
                 {
                     o.RegisterValidatorsFromAssemblyContaining<RoleValidator>();
                     o.RegisterValidatorsFromAssemblyContaining<POS_MasterValidator>();
                     o.RegisterValidatorsFromAssemblyContaining<CustomersInfoValidator>();
                     //o.RegisterValidatorsFromAssemblyContaining<Approve_ProcessValidator>();
                     o.RunDefaultMvcValidationAfterFluentValidationExecutes = false;

                 }
                )
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            // 取消默认驼峰
            .AddJsonOptions(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); }); ;

            #region 依赖注入

            var builder = new ContainerBuilder();//实例化容器
            //注册所有模块module
            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
            //获取所有的程序集
            //var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();
            var assemblys = RuntimeHelper.GetAllAssemblies().ToArray();

            //注册所有继承IDependency接口的类
            builder.RegisterAssemblyTypes().Where(type => typeof(IDependency).IsAssignableFrom(type) && !type.IsAbstract);
            //注册仓储，所有IRepository接口到Repository的映射
            builder.RegisterAssemblyTypes(assemblys).Where(t => t.Name.EndsWith("Repository") && !t.Name.StartsWith("I")).AsImplementedInterfaces();
            //注册服务，所有IApplicationService到ApplicationService的映射
            //builder.RegisterAssemblyTypes(assemblys).Where(t => t.Name.EndsWith("AppService") && !t.Name.StartsWith("I")).AsImplementedInterfaces();
            builder.Populate(services);
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer); //第三方IOC接管 core内置DI容器 
                                                                     //return services.BuilderInterceptableServiceProvider(builder => builder.SetDynamicProxyFactory());
            #endregion

           

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
            });
            #endregion
            app.UseCors("LimitRequests");//将 CORS 中间件添加到 web 应用程序管线中, 以允许跨域请求。
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
