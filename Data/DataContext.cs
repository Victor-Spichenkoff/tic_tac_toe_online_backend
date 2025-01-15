using asp_rest_model.Models;
using Microsoft.EntityFrameworkCore;

namespace asp_rest_model.Data;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}