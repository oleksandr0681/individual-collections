using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace IndividualCollectionsWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool Blocked { get; set; } = false;
        public bool DefaultPasswordChanged { get; set; } = true;

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class Theme
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Display(Name = "Тема")]
        public string Title { get; set; } = "";
    }

    public class Collection
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; } 

        [MaxLength(150)]
        [Display(Name = "Название коллекции")]
        public string Title { get; set; } = "";

        [MaxLength(1000)]
        [Display(Name = "Описание")]
        public string Description { get; set; } = "";

        public int ThemeId { get; set; } 
        public Theme Theme { get; set; } 

        [DataType(DataType.ImageUrl)]
        public string PictureUrl { get; set; } = "";

        [Display(Name = "Числовое поле 1 включено")]
        public bool IsNumerical1Enabled { get; set; } = false;

        [MaxLength(100)]
        [Display(Name = "Название числового поля 1")]
        public string Numerical1Title { get; set; } = "";

        [Display(Name = "Числовое поле 2 включено")]
        public bool IsNumerical2Enabled { get; set; } = false;

        [MaxLength(100)]
        [Display(Name = "Название числового поля 2")]
        public string Numerical2Title { get; set; } = "";

        [Display(Name = "Числовое поле 3 включено")]
        public bool IsNumerical3Enabled { get; set; } = false;

        [MaxLength(100)]
        [Display(Name = "Название числового поля 3")]
        public string Numerical3Title { get; set; } = "";

        [Display(Name = "Однострочное поле 1 включено")]
        public bool IsString1Enabled { get; set; } = false;

        [MaxLength(100)]
        [Display(Name = "Название однострочного поля 1")]
        public string String1Title { get; set; } = "";

        [Display(Name = "Однострочное поле 2 включено")]
        public bool IsString2Enabled { get; set; } = false;

        [MaxLength(100)]
        [Display(Name = "Название однострочного поля 2")]
        public string String2Title { get; set; } = "";

        [Display(Name = "Однострочное поле 3 включено")]
        public bool IsString3Enabled { get; set; } = false;

        [MaxLength(100)]
        [Display(Name = "Название однострочного поля 3")]
        public string String3Title { get; set; } = "";

        [Display(Name = "Текстовое поле 1 включено")]
        public bool IsText1Enabled { get; set; } = false;

        [MaxLength(100)]
        [Display(Name = "Название текстового поля 1")]
        public string Text1Title { get; set; } = "";

        [Display(Name = "Текстовое поле 2 включено")]
        public bool IsText2Enabled { get; set; } = false;

        [MaxLength(100)]
        [Display(Name = "Название текстового поля 2")]
        public string Text2Title { get; set; } = "";

        [Display(Name = "Текстовое поле 3 включено")]
        public bool IsText3Enabled { get; set; } = false;

        [MaxLength(100)]
        [Display(Name = "Название текстового поля 3")]
        public string Text3Title { get; set; } = "";

        [Display(Name = "Дата 1 включена")]
        public bool IsDate1Enabled { get; set; } = false;

        [MaxLength(100)]
        [Display(Name = "Название даты 1")]
        public string Date1Title { get; set; } = "";

        [Display(Name = "Дата 2 включена")]
        public bool IsDate2Enabled { get; set; } = false;

        [MaxLength(100)]
        [Display(Name = "Название даты 2")]
        public string Date2Title { get; set; } = "";

        [Display(Name = "Дата 3 включена")]
        public bool IsDate3Enabled { get; set; } = false;

        [MaxLength(100)]
        [Display(Name = "Название даты 3")]
        public string Date3Title { get; set; } = "";

        [Display(Name = "Чек-бокс 1 включен")]
        public bool IsBoolean1Enabled { get; set; } = false;

        [MaxLength(100)]
        [Display(Name = "Название чек-бокса 1")]
        public string Boolean1Title { get; set; } = "";

        [Display(Name = "Чек-бокс 2 включен")]
        public bool IsBoolean2Enabled { get; set; } = false;

        [MaxLength(100)]
        [Display(Name = "Название чек-бокса 2")]
        public string Boolean2Title { get; set; } = "";

        [Display(Name = "Чек-бокс 3 включен")]
        public bool IsBoolean3Enabled { get; set; } = false;

        [MaxLength(100)]
        [Display(Name = "Название чек-бокса 3")]
        public string Boolean3Title { get; set; } = "";

        public ICollection<Item> Items { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }

        public int CollectionId { get; set; } 
        public Collection Collection { get; set; } 

        [MaxLength(150)]
        [Display(Name = "Название айтема")]
        public string Title { get; set; } = "";

        [MaxLength(200)]
        [Display(Name = "Тэги")]
        public string Tags { get; set; } = "";

        [Display(Name = "Время добавления")]
        public DateTime? AddingTime { get; set; } = DateTime.Now;

        public decimal? Numerical1 { get; set; } = 0;

        public decimal? Numerical2 { get; set; } = 0;

        public decimal? Numerical3 { get; set; } = 0;

        [MaxLength(300)]
        public string String1 { get; set; } = "";

        [MaxLength(300)]
        public string String2 { get; set; } = "";

        [MaxLength(300)]
        public string String3 { get; set; } = "";

        [MaxLength(2000), DataType(DataType.MultilineText)]
        public string Text1 { get; set; } = "";

        [MaxLength(2000), DataType(DataType.MultilineText)]
        public string Text2 { get; set; } = "";

        [MaxLength(2000), DataType(DataType.MultilineText)]
        public string Text3 { get; set; } = "";

        [DataType(DataType.Date)]
        public DateTime? Date1 { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        public DateTime? Date2 { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        public DateTime? Date3 { get; set; } = DateTime.Today;

        public bool? Boolean1 { get; set; } = false;

        public bool? Boolean2 { get; set; } = false;

        public bool? Boolean3 { get; set; } = false;

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Like> Likes { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; } 

        public int ItemId { get; set; } 
        public Item Item { get; set; } 

        [MaxLength(1000), DataType(DataType.Text)]
        public string Note { get; set; } = "";
    }

    public class Like
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; } 

        public int ItemId { get; set; } // Имя класса и ID дают FOREIGN KEY (внешний ключ).
        public Item Item { get; set; } // Связь с таблицей Items

        public bool IsLiked { get; set; } = false;
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer(new ApplicationDbInitializer());
        }

        public DbSet<Theme> Themes { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            ApplicationUserManager userMaganer = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            IdentityRole roleAdministrator = new IdentityRole { Name = "administrator" };
            if (!roleManager.RoleExists(roleAdministrator.Name))
            {
                roleManager.Create(roleAdministrator);
            }
            ApplicationUser administrator = new ApplicationUser {
                UserName = "adminemail@test.com", Email = "adminemail@test.com",
                Blocked = false, DefaultPasswordChanged = false };
            string password = "Qwerty+1";
            IdentityResult result = userMaganer.Create(administrator, password);
            if (result.Succeeded)
            {
                userMaganer.AddToRole(administrator.Id, roleAdministrator.Name);
            }
            Theme themeBooks = new Theme { Title = "Книги" };
            Theme themeArt = new Theme { Title = "Произведения искусства" };
            Theme themePlants = new Theme { Title = "Растения" };
            Theme themeAnimals = new Theme { Title = "Животные" };
            context.Themes.Add(themeBooks);
            context.Themes.Add(themeArt);
            context.Themes.Add(themePlants);
            context.Themes.Add(themeAnimals);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}