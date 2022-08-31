using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using WpfExamProject.Service.DTOs;
using WpfExamProject.Service.Interfacess;
using WpfExamProject.Service.Servicess;
using WpfExamProject.Wpf.UI.Windows;

namespace WpfExamProject.Wpf.UI.Pages
{
    /// <summary>
    /// Interaction logic for AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        public AddPage()
        {
            InitializeComponent();
        }

        private async void AddPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NewStudentFirstName.Text) &&
                !string.IsNullOrEmpty(NewStudentLastName.Text) &&
                !string.IsNullOrEmpty(NewStudentFaculty.Text))
            {

                var newStudent = new StudentForCreationDto()
                {
                    FirstName = NewStudentFirstName.Text,
                    LastName = NewStudentLastName.Text,
                    Faculty = NewStudentFaculty.Text
                };

                using IStudentService studentService = new StudentService();

                var response = await studentService.CreateAsync(newStudent);

                if (response is null)
                {
                    MassageWindow massageWindow = new MassageWindow();
                    massageWindow.MassageWindowTextBox.Text = "Somthing went wrong";
                    massageWindow.Show();
                }
                else
                {
                    MassageWindow massageWindow = new MassageWindow();
                    massageWindow.Show();
                }
            }
            else
            {
                MassageWindow massageWindow = new MassageWindow();
                massageWindow.MassageWindowTextBox.Text = "Please fill all field";
                massageWindow.Show();
            }

        }

    }
}