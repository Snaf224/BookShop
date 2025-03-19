using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace kurs
{
    public class DbContextFactory
    {
        public class BooksDbContextFactory : IDesignTimeDbContextFactory<BooksDbContext>
        {
            public BooksDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<BooksDbContext>();

                // Подключение к базе данных
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json") // Исправлено название файла настроек
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                return new BooksDbContext(optionsBuilder.Options);
            }
        }

    }
}
