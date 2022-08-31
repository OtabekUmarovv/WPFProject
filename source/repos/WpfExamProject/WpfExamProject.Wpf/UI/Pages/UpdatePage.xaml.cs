using Microsoft.Win32;
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
    /// Interaction logic for UpdatePage.xaml
    /// </summary>
    public partial class UpdatePage : Page
    {
        private string imagePath;
        private string passportImagePath;
        
        public UpdatePage()
        {
            InitializeComponent();
        }



        private void PasspostImageBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "Image Files(*.PNG,*.JPG;)|*.JPG;*.PNG",
                InitialDirectory = Environment.GetFolderPath
                (Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                passportImagePath = openFileDialog.FileName;

                PassportImage.Text = passportImagePath.Split('\\').Last();
            }
        }

        private void ImageBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "Image Files(*.PNG,*.JPG;)|*.JPG;*.PNG",

                InitialDirectory = Environment.GetFolderPath
                (Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() is true)
            {
                imagePath = openFileDialog.FileName;

                Image.Text = imagePath.Split('\\').Last();
            }
        }

        private async void UpdatePageBtn_Click(object sender, RoutedEventArgs e)
        {

            var IsValidId = long.TryParse(UpdatedStudentId.Text, out long result);

            if (IsValidId)
            {
                using IStudentService studentService = new StudentService();

                var oldStudent = await studentService.GetAsync(result);

                if (oldStudent is null)
                {
                    MassageWindow massageWindow = new MassageWindow();
                    massageWindow.MassageWindowTextBox.Text = "Student Not Found";
                    
                    massageWindow.Show();
                    return;
                }

                var updateStudentInfo = new StudentForCreationDto();

                if (!string.IsNullOrEmpty(UpdatedStudentFirstName.Text))
                    updateStudentInfo.FirstName = UpdatedStudentFirstName.Text;
                else
                    updateStudentInfo.FirstName = oldStudent.FirstName;

                if (!string.IsNullOrEmpty(UpdatedStudentLastName.Text))
                    updateStudentInfo.LastName = UpdatedStudentLastName.Text;
                else
                    updateStudentInfo.LastName = oldStudent.LastName;

                if (!string.IsNullOrEmpty(UpdatedStudentFaculty.Text))
                    updateStudentInfo.Faculty = UpdatedStudentFaculty.Text;
                else
                    updateStudentInfo.Faculty = oldStudent.Faculty;

                if (imagePath != null && passportImagePath != null)
                    await studentService.UploadPicturesAsync(oldStudent.Id, imagePath, passportImagePath);

                if (imagePath != null && passportImagePath == null ||
                    imagePath == null && passportImagePath != null)
                {
                    //ErrorResponse.Text = "Please upload both images";
                    //ErrorResponse.Visibility = Visibility.Visible;
                    MassageWindow massageWindow = new MassageWindow();
                    massageWindow.MassageWindowTextBox.Text = "Please upload both images";
                    massageWindow.Show();

                    return;
                }

                var response = await studentService.UpdateAsync(result, updateStudentInfo);


                if (response is not null)
                {
                    MassageWindow massageWindow = new();
                    massageWindow.Show();
                }
            }
            else
            {
                MassageWindow massageWindow = new MassageWindow();
                massageWindow.MassageWindowTextBox.Text = "Please enter valid ID";
                massageWindow.Show();
            }
        }
    }
}
