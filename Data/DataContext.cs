using joinedTable.PokemonCategory;
using joinedTable.PokemonOwner;
using Microsoft.EntityFrameworkCore;
using Models.Category;
using Models.Country;
using Models.Owner;
using Models.pokemeon;
using Models.Review;
using Models.Reviewer;

namespace Data.DataContext{
    public class DataContext: DbContext{
        public DataContext(DbContextOptions<DataContext> options) : base(options){

        }

        public DbSet<Category> Categories {get; set;}
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners{ get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Establishing the many-to-many relationship for our Pokenmon & Category tables
            modelBuilder.Entity<PokemonCategory>().HasKey(pc => new {pc.PokemonId, pc.CategoryId});
            modelBuilder.Entity<PokemonCategory>()
            .HasOne(p => p.Pokemon)
            .WithMany(pc => pc.PokemonCategories)
            .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonCategory>()
            .HasOne(p => p.Category)
            .WithMany(pc => pc.PokemonCategories)
            .HasForeignKey(c => c.CategoryId);

            /////////////////////////////////////////////////////////////////////////////////////

            //Establishing the many-to-many relationship for our Pokenmon & Owner tables
            modelBuilder.Entity<PokemonOwner>().HasKey(po => new {po.PokemonId, po.OwnerId});
            modelBuilder.Entity<PokemonOwner>()
            .HasOne(p => p.Pokemon)
            .WithMany(po => po.PokemonOwners)
            .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonOwner>()
            .HasOne(p => p.Owner)
            .WithMany(po => po.PokemonOwners)
            .HasForeignKey(o => o.OwnerId);

        }
    }
}