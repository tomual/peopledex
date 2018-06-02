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

namespace peopledex
{
    /// <summary>
    /// Interaction logic for NewProfile.xaml
    /// </summary>
    public partial class NewProfile : Window
    {
        public NewProfile()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateNewProfileForm())
            {
                Profile profile = new Profile();
                profile.Id = "#014";
                profile.Name = NameInput.Text;
                profile.Picture = PictureInput.Text;
                profile.Location = LocationInput.Text;
                profile.Occupation = OccupationInput.Text;
                profile.Birthday = BirthdayInput.Text;
                profile.Likes = LikeInput.Text;
                profile.Description = DescriptionInput.Text;

                MainWindow main = (MainWindow)Application.Current.MainWindow;
                main.AddProfile(profile);
                main.SetProfile(profile);

                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool ValidateNewProfileForm()
        {
            bool valid = true;
            List<TextBox> required = new List<TextBox>();
            required.Add(NameInput);
            required.Add(LocationInput);
            required.Add(DescriptionInput);

            foreach(TextBox requiredInput in required)
            {
                requiredInput.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFABADB3"));
                ErrorLabel.Visibility = Visibility.Hidden;
                if (string.IsNullOrEmpty(requiredInput.Text))
                {
                    valid = false;
                    ErrorLabel.Visibility = Visibility.Visible;
                    requiredInput.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#D26759"));
                }
            }

            return valid;
        }
    }
}
