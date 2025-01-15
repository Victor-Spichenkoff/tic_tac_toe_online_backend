using asp_rest_model.Models;

namespace asp_rest_model.Interface.Repositoy;

public interface IUserRepository
{
    public List<User> GetAll();
}