using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace StFrancisHouse.Models
{
    public class UserContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=stfrancis;user=root;password=root238!");
        }
        public UserContext(DbContextOptions<UserContext> options)
            :base(options)
        {
        }

        //commented the below lone of code since it was not letting the rest of the code to compile
        //public DbSet<User> UserItems { get; set; } = null!;
    }
}
