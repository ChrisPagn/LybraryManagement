using LybraryManagement.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LybraryManagement.Server.Infrastructure.Data;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Subcategory> Subcategories => Set<Subcategory>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // categories
        modelBuilder.Entity<Category>(e =>
        {
            e.ToTable("categories");
            e.HasKey(x => x.CategoryId).HasName("PRIMARY");
            e.Property(x => x.CategoryId).HasColumnName("category_id");
            e.Property(x => x.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            e.Property(x => x.Description).HasColumnName("description");
            e.HasIndex(x => x.Name).IsUnique();
        });

        // subcategories
        modelBuilder.Entity<Subcategory>(e =>
        {
            e.ToTable("subcategories");
            e.HasKey(x => x.SubcategoryId).HasName("PRIMARY");
            e.Property(x => x.SubcategoryId).HasColumnName("subcategory_id");
            e.Property(x => x.CategoryId).HasColumnName("category_id");
            e.Property(x => x.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            e.Property(x => x.Description).HasColumnName("description");
            e.HasIndex(x => x.Name).IsUnique();

            e.HasOne(x => x.Category)
                .WithMany(c => c.Subcategories)
                .HasForeignKey(x => x.CategoryId)
                .HasConstraintName("subcategories_ibfk_1")
                .OnDelete(DeleteBehavior.NoAction);
        });

        // items
        modelBuilder.Entity<Item>(e =>
        {
            e.ToTable("items");
            e.HasKey(x => x.ItemId).HasName("PRIMARY");
            e.Property(x => x.ItemId).HasColumnName("item_id");
            e.Property(x => x.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
            e.Property(x => x.Creator).HasColumnName("creator").HasMaxLength(255);
            e.Property(x => x.Publisher).HasColumnName("publisher").HasMaxLength(255);
            e.Property(x => x.Year).HasColumnName("year").HasColumnType("year");
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.SubcategoryId).HasColumnName("subcategory_id");
            e.Property(x => x.DateAdded).HasColumnName("date_added").HasColumnType("datetime");
            e.Property(x => x.ImageUrl).HasColumnName("image_url").HasMaxLength(512);

            e.HasOne(x => x.Subcategory)
                .WithMany(s => s.Items)
                .HasForeignKey(x => x.SubcategoryId)
                .HasConstraintName("items_ibfk_1")
                .OnDelete(DeleteBehavior.NoAction);
        });

        // genres
        modelBuilder.Entity<Genre>(e =>
        {
            e.ToTable("genres");
            e.HasKey(x => x.GenreId).HasName("PRIMARY");
            e.Property(x => x.GenreId).HasColumnName("genre_id");
            e.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            e.HasIndex(x => x.Name).IsUnique();
        });

        // books
        modelBuilder.Entity<Book>(e =>
        {
            e.ToTable("books");
            e.HasKey(x => x.BookId).HasName("PRIMARY");
            e.Property(x => x.BookId).HasColumnName("book_id");
            e.Property(x => x.ItemId).HasColumnName("item_id").IsRequired();
            e.Property(x => x.GenreId).HasColumnName("genre_id");
            e.Property(x => x.Isbn).HasColumnName("isbn").HasMaxLength(20);

            e.HasIndex(x => x.ItemId).IsUnique();
            e.HasIndex(x => x.Isbn).IsUnique();

            e.HasOne(x => x.Item)
                .WithOne(i => i.Book)
                .HasForeignKey<Book>(x => x.ItemId)
                .HasConstraintName("books_ibfk_1")
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(x => x.GenreId)
                .HasConstraintName("books_ibfk_2")
                .OnDelete(DeleteBehavior.NoAction);
        });

        // games
        modelBuilder.Entity<Game>(e =>
        {
            e.ToTable("games");
            e.HasKey(x => x.GameId).HasName("PRIMARY");
            e.Property(x => x.GameId).HasColumnName("game_id");
            e.Property(x => x.ItemId).HasColumnName("item_id").IsRequired();
            e.Property(x => x.Platform).HasColumnName("platform").HasMaxLength(255);
            e.Property(x => x.AgeRange).HasColumnName("age_range").HasMaxLength(50);

            e.HasIndex(x => x.ItemId).IsUnique();

            e.HasOne(x => x.Item)
                .WithOne(i => i.Game)
                .HasForeignKey<Game>(x => x.ItemId)
                .HasConstraintName("games_ibfk_1")
                .OnDelete(DeleteBehavior.Cascade);
        });

        // movies
        modelBuilder.Entity<Movie>(e =>
        {
            e.ToTable("movies");
            e.HasKey(x => x.MovieId).HasName("PRIMARY");
            e.Property(x => x.MovieId).HasColumnName("movie_id");
            e.Property(x => x.ItemId).HasColumnName("item_id").IsRequired();
            e.Property(x => x.Duration).HasColumnName("duration").HasColumnType("time");
            e.Property(x => x.Rating).HasColumnName("rating").HasMaxLength(10);

            e.HasIndex(x => x.ItemId).IsUnique();

            e.HasOne(x => x.Item)
                .WithOne(i => i.Movie)
                .HasForeignKey<Movie>(x => x.ItemId)
                .HasConstraintName("movies_ibfk_1")
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

