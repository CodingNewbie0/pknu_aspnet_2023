using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using aspnet02_boardapp.Models;
using Microsoft.EntityFrameworkCore;

namespace aspnet02_boardapp.Data
{
    public class ApplicationDbContext : IdentityDbContext // 1. ASP.NET Identity : 결국 DbContext 쓰는것과 동일
    {
        public ApplicationDbContext(DbContextOptions option) : base(option) 
        {
        }
        public DbSet<Board> Boards { get; set; }
    }
}
