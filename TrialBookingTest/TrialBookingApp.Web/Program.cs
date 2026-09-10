using Microsoft.EntityFrameworkCore;
using TrialBookingApp.Web.Business.Interfaces.Repositories;
using TrialBookingApp.Web.Business.Interfaces.Services;
using TrialBookingApp.Web.Business.Services;
using TrialBookingApp.Web.DataAccess;
using TrialBookingApp.Web.DataAccess.Repositories;

namespace TrialBookingApp.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("TrialBookingDb")));

            builder.Services.AddScoped<IParentRepository, ParentRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<ITrialClassRepository, TrialClassRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IPaymentAttemptRepository, PaymentAttemptRepository>();

            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IPaymentService, MockPaymentService>();
            builder.Services.AddScoped<IParentService, ParentService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<ITrialClassService, TrialClassService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
