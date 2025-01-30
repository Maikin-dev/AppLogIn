namespace API_LOGIN
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configurar servicios
            builder.Services.AddControllers();

            // Configuraci�n de Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configurar CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermitirFrontend", policy =>
                {
                    policy.WithOrigins("http://127.0.0.1:5500") // Direcci�n del frontend permitido
                          .AllowAnyMethod()                     // Permitir todos los m�todos HTTP
                          .AllowAnyHeader();                   // Permitir todos los encabezados
                });
            });

            var app = builder.Build();

            // Configurar el pipeline de solicitudes HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Aplicar el middleware de CORS
            app.UseCors("PermitirFrontend");

            app.UseAuthorization();

            // Mapear controladores al pipeline
            app.MapControllers();

            // Ejecutar la aplicaci�n
            app.Run();
        }
    }
}
