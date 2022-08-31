using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfExamProject.Domain.Entities;
using WpfExamProject.Service.DTOs;
using WpfExamProject.Service.Interfacess;
using WpfExamProject.Service.Servicess;
using WpfExamProject.Wpf.UI.Controls;
using WpfExamProject.Wpf.UI.Pages;

namespace WpfExamProject.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AddPage addPage;

        private readonly UpdatePage updatePage;
        
        private readonly DeletePage deletePage;
        
        private readonly IStudentService studentService;
        
        private Thread thread;

       
        public MainWindow()
        {
            studentService = new StudentService();

            addPage = new AddPage();
            
            updatePage = new UpdatePage();
            
            deletePage = new DeletePage();
            
            InitializeComponent();
        }

       
        private IEnumerable<Student> allStudents;

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            thread = new Thread(async () =>
            {
                Dispatcher.Invoke(() => StudentsList.Items.Clear());

                allStudents = await studentService.GetAllAsync();

                await LoadStudents(allStudents);
            });

            thread.Start();
        }

        private async void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            StudentsList.Items.Clear();

            string text = SearchBox.Text.ToLower();


            thread = new Thread(async () =>
            {
                allStudents = await studentService.GetAllAsync();

                allStudents = allStudents.Where(p => p.FirstName.ToLower().Contains(text)
                    || p.LastName.ToLower().Contains(text)
                    || p.Faculty.ToLower().Contains(text));

                await LoadStudents(allStudents);
            });
            thread.Start();
        }

        private async Task LoadStudents(IEnumerable<Student> students)
        {
            foreach (var student in students)
            {
                await this.Dispatcher.InvokeAsync(() =>
                {
                    StudentInfo studentInfo = new StudentInfo();

                    studentInfo.NameTxt.Text = student.FirstName + " " + student.LastName;
                    studentInfo.Faculty.Text = student.Faculty;

                    studentInfo.StudentImage.ImageSource = student.Image is not null
                        ? new BitmapImage(new Uri("https://talabamiz.uz/" + student.Image.Path))
                        : new BitmapImage(new Uri("https://talabamiz.uz/Images//99daf8ac38de4433aa36a61baf4c9c4d.png"));

                    StudentsList.Items.Add(studentInfo);
                });
            }
        }

        private void Updatebtn_Click(object sender, RoutedEventArgs e)
        {
            EnterPool.Content = new UpdatePage();
        }

        private void Addbtn_Click(object sender, RoutedEventArgs e)
        {
            EnterPool.Content = new AddPage();
        }

        private void Deletebtn_Click(object sender, RoutedEventArgs e)
        {
            EnterPool.Content = new DeletePage();
        }

    }
}
