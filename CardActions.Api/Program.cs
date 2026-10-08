using CardActions.Api.Services;
using Microsoft.OpenApi.Models;

namespace CardActions.Api
{
    /// <summary>
    /// Application entry point and composition root.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Configures services, the HTTP pipeline and runs the application.
        /// </summary>
        /// <param name="args">Command line arguments, for example <c>--urls</c>.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddProblemDetails();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Card Actions API",
                    Version = "v1",
                    Description = "Returns actions allowed for a user's payment card. To see matrix of possible proper configurations check file sample-cards."
                });

                var xmlDocumentationPath = Path.Combine(AppContext.BaseDirectory, $"{typeof(Program).Assembly.GetName().Name}.xml");
                if (File.Exists(xmlDocumentationPath))
                {
                    options.IncludeXmlComments(xmlDocumentationPath);
                }
            });

            builder.Services.AddSingleton<ICardService, CardService>();
            builder.Services.AddSingleton<IUserCardService, UserCardService>();


            var app = builder.Build();

            app.UseExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
