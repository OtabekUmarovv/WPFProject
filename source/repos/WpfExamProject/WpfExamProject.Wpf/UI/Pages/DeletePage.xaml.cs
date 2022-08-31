using Amazon.Runtime.Internal;
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
using System.Windows.Shapes;
using WpfExamProject.Service.Interfacess;
using WpfExamProject.Service.Servicess;
using WpfExamProject.Wpf.UI.Windows;

namespace WpfExamProject.Wpf.UI.Pages
{
    /// <summary>
    /// Interaction logic for DeletePage.xaml
    /// </summary>
    public partial class DeletePage : Page
    {
        public DeletePage()
        {
            InitializeComponent();
        }
        private async void DeletePageBtn_Click(object sender, RoutedEventArgs e)
        {
            var IsValidId = long.TryParse(DeletedStudentId.Text, out long result);

            if (IsValidId)
            {
                using IStudentService studentService = new StudentService();

                var response = await studentService.DeleteAsync(result);

                if (!response)
                {
                    MassageWindow massageWindow = new MassageWindow();
                    massageWindow.MassageWindowTextBox.Text = "Student Not Found";
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
                //ErrorResponse.Text = "Invalid Id";
                //ErrorResponse.Visibility = Visibility.Visible;
            }

        }
    }
}
