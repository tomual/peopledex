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
            Profile profile = new Profile();
            profile.Id = "#014";
            profile.Name = NameInput.Text;
            profile.Picture = PictureInput.Text;
            profile.Location = LocationInput.Text;
            profile.Occupation = OccupationInput.Text;
            profile.Birthday = BirthdayInput.Text;
            profile.Likes = LikeInput.Text;
            profile.Dislikes = DislikesInput.Text;
            profile.Description = DescriptionInput.Text;

            MainWindow main = (MainWindow)Application.Current.MainWindow;
            main.AddProfile(profile);
            main.SetProfile(profile);


            this.Close();
        }
    }
}
