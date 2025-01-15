using asp_rest_model.Data;
using asp_rest_model.Interface.Repositoy;
using asp_rest_model.Models;

namespace asp_rest_model.Services.Repositories;

public class UserRepository(DataContext context): IUserRepository
{
    private readonly DataContext _context = context;
    
    public List<User> GetAll()
    {
        return _context.Users.ToList();
    }
}