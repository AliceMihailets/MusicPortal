using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MusicPortal.Models
{
    public class musicalportalcontext : DbContext
    {
        public DbSet<Пользователь> Пользователь { get; set; }
        public DbSet<Альбомы> Альбомы { get; set; }
        public DbSet<Жанры> Жанры { get; set; }
        public DbSet<Исполнители> Исполнители { get; set; }
        public DbSet<Исполнители_в_коллективах> Исполнители_в_коллективах { get; set; }
        public DbSet<Коллективы> Коллективы { get; set; }
        public DbSet<Награды> Награды { get; set; }
        public DbSet<Награды_альбомов> Награды_альбомов { get; set; }
        public DbSet<Плейлист> Плейлист { get; set; }
        public DbSet<Премии> Премии { get; set; }
        public DbSet<Премии_коллективов> Премии_Коллективов { get; set; }
        public DbSet<Роли_в_коллективах> Роли_в_коллективах { get; set; }
        public DbSet<Типы_альбомов> Типы_альбомов { get; set; }
        public DbSet<Типы_коллективов> Типы_коллективов { get; set; }
        public DbSet<Треки> Треки { get; set; }
        public DbSet<Треки_в_плейлисте> Треки_в_плейлисте { get; set; }
        public musicalportalcontext(DbContextOptions<musicalportalcontext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Исполнители_в_коллективах>().ToTable("Исполнители в коллективах");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Награды_альбомов>().ToTable("Награды альбомов");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Премии_коллективов>().ToTable("Премии коллективов");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Роли_в_коллективах>().ToTable("Роли в коллективах");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Типы_альбомов>().ToTable("Типы альбомов");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Типы_коллективов>().ToTable("Типы коллективов");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Треки_в_плейлисте>().ToTable("Треки в плейлисте");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Пользователь>().Property(p => p.ID).HasColumnName("ID Пользователя");
            modelBuilder.Entity<Плейлист>().Property(p => p.ID_Пользователя).HasColumnName("ID Пользователя");
            modelBuilder.Entity<Плейлист>().Property(p => p.ID).HasColumnName("ID Плейлиста");
            modelBuilder.Entity<Треки_в_плейлисте>().Property(p => p.ID_Плейлиста).HasColumnName("ID Плейлиста");
            modelBuilder.Entity<Треки_в_плейлисте>().Property(p => p.ID_Трека).HasColumnName("ID Трека");
            modelBuilder.Entity<Треки>().Property(p => p.ID).HasColumnName("ID трека");
            modelBuilder.Entity<Треки>().Property(p => p.ID_альбома).HasColumnName("ID альбома");
            modelBuilder.Entity<Треки>().Property(p => p.Название_трека).HasColumnName("Название трека");
            modelBuilder.Entity<Альбомы>().Property(p => p.ID).HasColumnName("ID альбома");
            modelBuilder.Entity<Альбомы>().Property(p => p.ID_Жанра).HasColumnName("ID жанра");
            modelBuilder.Entity<Альбомы>().Property(p => p.ID_коллектива).HasColumnName("ID коллектива");
            modelBuilder.Entity<Альбомы>().Property(p => p.ID_Типа_альбома).HasColumnName("ID типа альбома");
            modelBuilder.Entity<Альбомы>().Property(p => p.Название_альбома).HasColumnName("Название альбома");
            modelBuilder.Entity<Альбомы>().Property(p => p.Дата_выпуска).HasColumnName("Дата выпуска");
            modelBuilder.Entity<Награды_альбомов>().Property(p => p.ID_альбома).HasColumnName("ID альбома");
            modelBuilder.Entity<Награды_альбомов>().Property(p => p.ID_награды).HasColumnName("ID награды");
            modelBuilder.Entity<Награды_альбомов>().Property(p => p.Год_получения).HasColumnName("Год получения");
            modelBuilder.Entity<Награды>().Property(p => p.ID_награды).HasColumnName("ID награды");
            modelBuilder.Entity<Награды>().Property(p => p.Название_награды).HasColumnName("Название награды");
            modelBuilder.Entity<Жанры>().Property(p => p.ID_жанра).HasColumnName("ID жанра");
            modelBuilder.Entity<Типы_альбомов>().Property(p => p.ID_типа_альбомы).HasColumnName("ID типа альбома");
            modelBuilder.Entity<Типы_альбомов>().Property(p => p.Тип_альбома).HasColumnName("Тип альбома");
            modelBuilder.Entity<Коллективы>().Property(p => p.ID).HasColumnName("ID коллектива");
            modelBuilder.Entity<Коллективы>().Property(p => p.Название_коллектива).HasColumnName("Название коллектива");
            modelBuilder.Entity<Коллективы>().Property(p => p.Дата_образования).HasColumnName("Дата образования");
            modelBuilder.Entity<Коллективы>().Property(p => p.Дата_расспада).HasColumnName("Дата распада");
            modelBuilder.Entity<Коллективы>().Property(p => p.ID_типа_коллектива).HasColumnName("ID типа коллектива");
            modelBuilder.Entity<Премии_коллективов>().Property(p => p.ID_коллектива).HasColumnName("ID коллектива");
            modelBuilder.Entity<Премии_коллективов>().Property(p => p.ID_премии).HasColumnName("ID премии");
            modelBuilder.Entity<Премии_коллективов>().Property(p => p.Год_вручения).HasColumnName("Год вручения");
            modelBuilder.Entity<Премии>().Property(p => p.ID).HasColumnName("ID премии");
            modelBuilder.Entity<Премии>().Property(p => p.Название_премии).HasColumnName("Название премии");
            modelBuilder.Entity<Типы_коллективов>().Property(p => p.ID_типа_коллектива).HasColumnName("ID типа коллектива");
            modelBuilder.Entity<Типы_коллективов>().Property(p => p.Тип_коллектива).HasColumnName("Тип коллектива");
            modelBuilder.Entity<Исполнители_в_коллективах>().Property(p => p.ID_исполнителя).HasColumnName("ID исполнителя");
            modelBuilder.Entity<Исполнители_в_коллективах>().Property(p => p.ID_коллектива).HasColumnName("ID коллектива");
            modelBuilder.Entity<Исполнители_в_коллективах>().Property(p => p.ID_роли_в_коллективе).HasColumnName("ID роли в коллективе");
            modelBuilder.Entity<Исполнители_в_коллективах>().Property(p => p.Дата_начала_работы_в_коллективе).HasColumnName("Дата начала работы в коллективе");
            modelBuilder.Entity<Исполнители_в_коллективах>().Property(p => p.Дата_окончания_работы_в_коллективе).HasColumnName("Дата окончания работы в коллективе");
            modelBuilder.Entity<Роли_в_коллективах>().Property(p => p.ID_роли_в_коллективе).HasColumnName("ID роли в коллективе");
            modelBuilder.Entity<Роли_в_коллективах>().Property(p => p.Название_роли).HasColumnName("Название роли");
            modelBuilder.Entity<Исполнители>().Property(p => p.ID_исполнителя).HasColumnName("ID исполнителя");

            modelBuilder.Entity<Награды_альбомов>().HasKey(p => new { p.ID_награды, p.ID_альбома });
            modelBuilder.Entity<Треки_в_плейлисте>().HasKey(p => new { p.ID_Плейлиста, p.ID_Трека });
            modelBuilder.Entity<Исполнители_в_коллективах>().HasKey(p => new { p.ID_исполнителя, p.ID_коллектива });
            modelBuilder.Entity<Премии_коллективов>().HasKey(p => new { p.ID_премии, p.ID_коллектива });

            modelBuilder.Entity<Плейлист>().HasOne(p => p.Пользователь).WithMany(p => p.плейлисты).HasForeignKey(p => p.ID_Пользователя);
            modelBuilder.Entity<Треки_в_плейлисте>().HasOne(p => p.плейлист).WithMany(p => p.треки).HasForeignKey(p => p.ID_Плейлиста);
            modelBuilder.Entity<Треки_в_плейлисте>().HasOne(p => p.трек).WithMany(p => p.плейлиты).HasForeignKey(p => p.ID_Трека);
            modelBuilder.Entity<Награды_альбомов>().HasOne(p => p.Альбом).WithMany(p => p.награды).HasForeignKey(p => p.ID_альбома);
            modelBuilder.Entity<Награды_альбомов>().HasOne(p => p.Наград).WithMany(p => p.альбомы).HasForeignKey(p => p.ID_награды);
            modelBuilder.Entity<Альбомы>().HasOne(p => p.жанр).WithMany(p => p.альбомы).HasForeignKey(p => p.ID_Жанра);
            modelBuilder.Entity<Альбомы>().HasOne(p => p.тип_альбома).WithMany(p => p.альбомы).HasForeignKey(p => p.ID_Типа_альбома);
            modelBuilder.Entity<Альбомы>().HasOne(p => p.коллектив).WithMany(p => p.альбомы).HasForeignKey(p => p.ID_коллектива);
            modelBuilder.Entity<Коллективы>().HasOne(p => p.тип_Коллектива).WithMany(p => p.коллективы).HasForeignKey(p => p.ID_типа_коллектива);
            modelBuilder.Entity<Премии_коллективов>().HasOne(p => p.премия).WithMany(p => p.коллективы).HasForeignKey(p => p.ID_премии);
            modelBuilder.Entity<Премии_коллективов>().HasOne(p => p.коллектив).WithMany(p => p.премии).HasForeignKey(p => p.ID_коллектива);
            modelBuilder.Entity<Исполнители_в_коллективах>().HasOne(p => p.роли_В_Коллективах).WithMany(p => p.исполнители).HasForeignKey(p => p.ID_роли_в_коллективе);
            modelBuilder.Entity<Исполнители_в_коллективах>().HasOne(p => p.коллектив).WithMany(p => p.исполнители).HasForeignKey(p => p.ID_коллектива);
            modelBuilder.Entity<Исполнители_в_коллективах>().HasOne(p => p.исполнитель).WithMany(p => p.коллективы).HasForeignKey(p => p.ID_исполнителя);
            modelBuilder.Entity<Треки>().HasOne(p => p.Альбом).WithMany(p => p.треки).HasForeignKey(p => p.ID_альбома);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string configuring = "Data Source=localhost;Initial Catalog=8I7A_Mihailets_MusicDB;Integrated Security=True;User ID=SA;Password=Root$123;";
            string configuring = "Data Source=localhost;Initial Catalog=8I7A_Mihailets_MusicDB;Integrated Security=True;";
            optionsBuilder.UseSqlServer(configuring);
        }
    }
}
