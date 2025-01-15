using asp_rest_model.Data;
using Microsoft.EntityFrameworkCore;

namespace asp_rest_model.Helpers;

public static class MigrationManager
{
    public static WebApplication MigrateDatabase(this WebApplication webApp)
    {
        using (var scope = webApp.Services.CreateScope())
        {
            using (var appContext = scope.ServiceProvider.GetRequiredService<DataContext>())
            {
                try
                {
                    // atualizar o db
                    appContext.Database.Migrate();

                    // fazer o seeding
                    var seeding = new Seeding(appContext);

                    seeding.CreateAll();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("Erro ao aplicar as migartions/seed");
                    throw;
                }
            }
        }

        return webApp;
    }
}