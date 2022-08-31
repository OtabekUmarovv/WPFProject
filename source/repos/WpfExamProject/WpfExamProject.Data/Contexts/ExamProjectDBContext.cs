using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfExamProject.Domain.Entities;

namespace WpfExamProject.Data.Contexts
{
    public class ExamProjectDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Host=localhost;Port=5432;Database=ExamWPF;Username=postgres;Password=1379";
            optionsBuilder.UseNpgsql(connectionString);

        }

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
    }
}
