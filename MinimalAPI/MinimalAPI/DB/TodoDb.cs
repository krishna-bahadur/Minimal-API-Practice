using Microsoft.EntityFrameworkCore;
using MinimalAPI.Models;

namespace MinimalAPI.DB
{
    public class TodoDb : DbContext
    {
        public TodoDb(DbContextOptions<TodoDb> options) 
            : base(options){ }
        public DbSet<Todo> Todos => Set<Todo>();

    }
}
