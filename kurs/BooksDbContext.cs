//using kurs.Models;
//using Microsoft.EntityFrameworkCore;

//namespace kurs
//{
//    public class BooksDbContext : DbContext
//    {
//        public BooksDbContext(DbContextOptions<BooksDbContext> options) : base(options)
//        {
//        }

//        public DbSet<Author> Authors { get; set; }
//        public DbSet<Book> Books { get; set; }
//        public DbSet<Customer> Customers { get; set; }
//        public DbSet<Order> Orders { get; set; }
//        public DbSet<OrderStatus> OrderStatuses { get; set; }

//        public DbSet<Profile> Profiles { get; set; }
//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<OrderStatus>()
//            .HasKey(os => os.StatusId); // Настраиваем StatusId как первичный ключ
//            // Настройка точности для цены книги
//            modelBuilder.Entity<Book>()
//                .Property(b => b.Price)
//                .HasColumnType("decimal(18, 2)"); // Задаем точность и масштаб для поля Price

//            // Связь: Автор и Книги (1 ко многим)
//            modelBuilder.Entity<Book>()
//                .HasOne(b => b.Authors) // Книга имеет одного автора
//                .WithMany(a => a.Books) // У автора много книг
//                .HasForeignKey(b => b.AuthorId) // Внешний ключ в Book
//                .OnDelete(DeleteBehavior.Cascade); // Удаление автора удаляет книги

//            // Связь: Покупатель и Заказы (1 ко многим)
//            modelBuilder.Entity<Order>()
//                .HasOne(o => o.Customer) // У заказа один покупатель
//                .WithMany(c => c.Orders) // У покупателя много заказов
//                .HasForeignKey(o => o.CustomerId) // Внешний ключ в Order
//                .OnDelete(DeleteBehavior.Cascade); // Удаление покупателя удаляет его заказы

//            // Связь: Заказ и Статусы (1 ко многим)
//            modelBuilder.Entity<OrderStatus>()
//                .HasOne(os => os.Order) // У статуса один заказ
//                .WithMany(o => o.OrderStatuses) // У заказа много статусов
//                .HasForeignKey(os => os.OrderId) // Внешний ключ в OrderStatus
//                .OnDelete(DeleteBehavior.Cascade); // Удаление заказа удаляет статусы

//            base.OnModelCreating(modelBuilder);
//        }
//    }
//}

using kurs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace kurs
{
    public class BooksDbContext : IdentityDbContext<ApplicationUser>
    {
        // DbSet'ы для книг, заказов и других сущностей
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        // Конструктор с передачей параметра DbContextOptions
        public BooksDbContext(DbContextOptions<BooksDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация моделей Identity
            base.OnModelCreating(modelBuilder);

            var passHasher = new PasswordHasher<ApplicationUser>();

            // Настройка данных для ApplicationUser
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser()
                {
                    Id = "49cd49b5-7b64-4c2f-8249-2fdf08b6ed4a",
                    UserName = "admin@email.em",
                    NormalizedUserName = "ADMIN@EMAIL.EM",
                    Email = "admin@email.em",
                    NormalizedEmail = "ADMIN@EMAIL.EM",
                    PasswordHash = passHasher.HashPassword(null!, "Password123++"),
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Gender = "Male",
                    DateOfEmployment = new DateTime(2003,2,3),
                },
                new ApplicationUser()
                {
                    Id = "49cd49b5-7b64-4c2f-8249-2fdf08b6ed4b",
                    UserName = "user@email.em",
                    NormalizedUserName = "USER@EMAIL.EM",
                    Email = "user@email.em",
                    NormalizedEmail = "USER@EMAIL.EM",
                    PasswordHash = passHasher.HashPassword(null!, "Password123++"),
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Gender = "Male",
                    DateOfEmployment = new DateTime(2003, 2, 3),
                }
            );

            // Настройка ролей
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = "49cd49b5-7b64-4c2f-8249-2fdf08b6ed4c",
                    Name = "admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                },
                new IdentityRole()
                {
                    Id = "49cd49b5-7b64-4c2f-8249-2fdf08b6ed4d",
                    Name = "user",
                    NormalizedName = "USER",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                }
            );

            // Связь пользователей с ролями
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>()
                {
                    UserId = "49cd49b5-7b64-4c2f-8249-2fdf08b6ed4a",
                    RoleId = "49cd49b5-7b64-4c2f-8249-2fdf08b6ed4c"
                },
                new IdentityUserRole<string>()
                {
                    UserId = "49cd49b5-7b64-4c2f-8249-2fdf08b6ed4a",
                    RoleId = "49cd49b5-7b64-4c2f-8249-2fdf08b6ed4d"
                },
                new IdentityUserRole<string>()
                {
                    UserId = "49cd49b5-7b64-4c2f-8249-2fdf08b6ed4b",
                    RoleId = "49cd49b5-7b64-4c2f-8249-2fdf08b6ed4d"
                }
            );

            // Настройка других моделей, как в BooksDbContext
            modelBuilder.Entity<OrderStatus>()
                .HasKey(os => os.StatusId);

            modelBuilder.Entity<Book>()
                .Property(b => b.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Authors)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderStatus>()
                .HasOne(os => os.Order)
                .WithMany(o => o.OrderStatuses)
                .HasForeignKey(os => os.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}

