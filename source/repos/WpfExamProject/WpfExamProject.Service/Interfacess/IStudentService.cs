using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WpfExamProject.Domain.Entities;
using WpfExamProject.Service.DTOs;

namespace WpfExamProject.Service.Interfacess
{
    public interface IStudentService : IDisposable
    {
        Task<Student> GetAsync(long id);

        Task<IEnumerable<Student>> GetAllAsync();

        Task<Student> CreateAsync(StudentForCreationDto Student);

        Task<Student> UpdateAsync(long id, StudentForCreationDto Studen);

        Task<bool> DeleteAsync(long id);

        Task UploadPicturesAsync(long id, string imagePath, string passportPath);
    }
}
