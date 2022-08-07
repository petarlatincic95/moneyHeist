using Microsoft.EntityFrameworkCore;
using MoneyHeistAPI.DTO_Helper_classes;
using MoneyHeistAPI.Model;

namespace MoneyHeistAPI.Data
{
    public class HeistDbContext : DbContext
    {
        public HeistDbContext(DbContextOptions<HeistDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<HeistMember>().HasIndex(u => u.Email).IsUnique();
            builder.Entity<MemberSkill>().HasIndex(u => new { u.SkillName, u.SkillLevel }).IsUnique();
            builder.Entity<Heist>().HasIndex(u => u.Name).IsUnique();
            builder.Entity<HeistSkill>().HasIndex(u => new { u.SkillName, u.SkillLevel }).IsUnique();

        }
        public DbSet<HeistMember> HeistMembers { get; set; }
        public DbSet<HeistSkill> HeistSkills { get; set; }
        public DbSet<MemberSkill> MemberSkills { get; set; }
        public DbSet<Heist> Heists  {get;set;}
     
         
    }
    
}
