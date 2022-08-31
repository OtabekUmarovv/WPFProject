using Mapster;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WpfExamProject.Data.IRepositories;
using WpfExamProject.Data.Repositories;
using WpfExamProject.Domain.Constants;
using WpfExamProject.Domain.Entities;
using WpfExamProject.Service.DTOs;
using WpfExamProject.Service.Extensions;
using WpfExamProject.Service.Interfacess;

namespace WpfExamProject.Service.Servicess
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;

        private readonly HttpClient httpClient;

        private readonly IAttachmentRepository attachmentRepository;

        private readonly string url = ConstantApi.BASE_URL + "Students/";

        public StudentService()
        {
            attachmentRepository = new AttachmentRepository();
            studentRepository = new StudentRepository();

            httpClient = new HttpClient();
        }

        public async Task<Student> CreateAsync(StudentForCreationDto Student)
        {
            var newStudent = JsonConvert.SerializeObject(Student);

            var requestContent = new StringContent
                (newStudent, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync
                (url, requestContent);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var createdStudent = JsonConvert.DeserializeObject<Student>(content);

                await studentRepository.CreateAsync(createdStudent);

                return createdStudent;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var response = await httpClient.DeleteAsync(url + id);

            if (response.IsSuccessStatusCode)
            {
                if (id > 320)
                {
                    await studentRepository.DeleteAsync(p => p.Id == id);

                }

                return true;

            }
            return false;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("admin:12345")));

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var resultContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<Student>>(resultContent);
            }

            return null;

        }

        public async Task<Student> GetAsync(long id)
        {
            var response = await httpClient.GetAsync(url + id);

            if (response.IsSuccessStatusCode)
            {
                var resultContent = await response.Content.ReadAsStringAsync();

                resultContent = resultContent.Replace('\\', '/');

                return JsonConvert.DeserializeObject<Student>(resultContent);
            }

            return null;
        }

        public async Task<Student> UpdateAsync(long id, StudentForCreationDto Student)
        {
            var newStudent = JsonConvert.SerializeObject(Student);

            StringContent responseContent = new(newStudent,
                Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync
                            (url + id, responseContent);


            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                content = content.Replace('\\', '/');

                var updatedStudent = JsonConvert.DeserializeObject<Student>(content);

                studentRepository.Update(updatedStudent);

                return updatedStudent;
            }

            return null;
        }

        public async Task UploadPicturesAsync(long id, string imagePath, string passportPath)
        {
            using HttpClient client = new();

            MultipartFormDataContent formData = new();

            if (imagePath is not null)
            {
                formData.Add(new StreamContent(File.OpenRead(imagePath)), "image", "image.png");
            }
            if (passportPath is not null)
            {
                formData.Add(new StreamContent(File.OpenRead(passportPath)), "passport", "passport.png");
            }

            var isUploadedSucceccfully = await client.PostAsync(url + "attachments/" + id, formData);



            if (isUploadedSucceccfully.IsSuccessStatusCode)
            {
                var response = await GetAsync(id);

                await attachmentRepository.CreateAsync(response.Image);

                await attachmentRepository.CreateAsync(response.Passport);

                return;
            }

        }
    }
}
