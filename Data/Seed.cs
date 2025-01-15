using asp_rest_model.Models;


namespace asp_rest_model.Data;

public class Seeding(DataContext context)
{
    private readonly DataContext _context = context;

    public void CreateAll()
    {
        SeedingUser();

        var success = _context.SaveChanges() > 0;// precisa dele

        if (success)
            Console.WriteLine("[ SEED ] Tudo criado");
        else 
            Console.WriteLine("[ SEED ] Erro ao criar");
    }
    
    public void SeedingUser()
    {
        if (_context.Users.Any()) //validar se já não tem
        {
            Console.WriteLine("Usuário já existe, pulando...");
            return;
        }

        _context.Users.AddRange
        (
            new User
            {
                Name = "Victor",
            },
            new User
            {
                Name = "Testador 1",
            }
        );
    }
}