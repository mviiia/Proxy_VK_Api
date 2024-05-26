namespace Proxy_VK_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            }));

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseCors("corsapp");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}