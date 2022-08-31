using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WpfExamProject.Data.Contexts;
using WpfExamProject.Data.IRepositories;
using WpfExamProject.Domain.Entities;

namespace WpfExamProject.Data.Repositories
{
        public class StudentRepository : GenericRepository<Student>, IStudentRepository
        {
        }
}
