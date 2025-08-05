using AutoMapper;
using Notes.Application;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Persistence;
using System.Reflection;

namespace Notes.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAutoMapper(cfg =>
            //{
            //    cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            //    cfg.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
            //});  //.net5 работает там

            services.AddAutoMapper(Assembly.GetExecutingAssembly(),
                typeof(INotesDbContext).Assembly);//AutoMapper сам найдёт все классы, унаследованные от Profile, в этих сборках.

            services.AddApplication();
            services.AddPersistence(Configuration);
            services.AddControllers();

            services.AddCors(options =>
            { //то есть инчава факат а адреси худаш запрос нею а дигар чохоям запрос омданаш мумкин ин вактба
                options.AddPolicy("AllowAll", policy => //задаем правило: кто угодно как угодно может обращаться к нам в данном случае.
                {                                 //в реальном приложении можно и нужно   ограничить , чтобы предоставить разрешение на доступ определенным образом
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting(); // определяет куда должее пойти запрос
                              //Когда клиент отправляет HTTP-запрос, UseRouting():
                              //Читает URL запроса.
                              //Сравнивает его с зарегистрированными маршрутами(в MapControllerRoute, MapGet, [Route] и т.п.).
                              //Находит подходящий endpoint.
                              //Сохраняет информацию о маршруте в HttpContext, чтобы следующие middleware знали, какой обработчик следует вызвать.
            app.UseHttpsRedirection();//автоматически перенаправляет HTTP-запросы на HTTPS.
            app.UseCors("AllowAll");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
