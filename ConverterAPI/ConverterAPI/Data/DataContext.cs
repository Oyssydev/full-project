using System.Data;
using ConverterClassLib;
using Microsoft.EntityFrameworkCore;

namespace ConverterAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext>options) : base (options){ }
        //tables here !
        public DbSet<Etudiant> Etudiants { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
