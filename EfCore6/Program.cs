using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services
            .AddDbContext<MyDbContext>(options =>
            {
                options.UseSqlServer(hostContext.Configuration.GetConnectionString(nameof(MyDbContext)));
            });
    })
    .Build();

await host.RunAsync();

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(builder =>
        {
            builder
                .HasMany(x => x.Products)
                .WithOne()
                .HasForeignKey(x => x.CategoryId);
        });
    }
}

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<Product> Products { get; set; }
}

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int CategoryId { get; set; }
}

