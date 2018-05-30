using System;
using System.Collections;
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
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Profile> ProfileList;

        public MainWindow()
        {
            if (Properties.Settings.Default.ProfileList != null)
            {
                ProfileList = Properties.Settings.Default.ProfileList;
            }
            else
            {
                ProfileList = new List<Profile>();
            }

            InitializeComponent();
            InitializeProfile();
        }

        private void InitializeProfile()
        {
            if (Properties.Settings.Default.ProfileList != null)
            {
                foreach (Profile profile in ProfileList)
                {
                    ProfileListing.Items.Add(profile);
                    SetProfile(profile);
                }
            }
        }

        private void NewProfileButton_Click(object sender, RoutedEventArgs e)
        {
            NewProfile newProfile = new NewProfile();
            newProfile.Show();
        }

        public void AddProfile(Profile profile)
        {
            ProfileList.Add(profile);
            ProfileListing.Items.Add(profile);
            Properties.Settings.Default.ProfileList = ProfileList;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Test = profile.Name;
            Properties.Settings.Default.Save();
            PrintProfileList();
        }

        public void SetProfile(Profile profile)
        {
            ProfileNumber.Text = profile.Id;
            ProfileName.Text = profile.Name;
            ProfileImage.Source = new BitmapImage(new Uri(@"Resources\" + profile.Picture, UriKind.Relative));
            ProfileLocation.Text = profile.Location;
            ProfileOccupation.Text = profile.Occupation;
            ProfileBirthday.Text = profile.Birthday;
            ProfileLikes.Text = profile.Likes;
            ProfileDislikes.Text = profile.Dislikes;
            ProfileDescription.Text = profile.Description;
        }

        public void PrintProfileList()
        {
            foreach (Profile profile in Properties.Settings.Default.ProfileList)
            {
                Console.WriteLine(profile.Name);
            }
        }

        private void ProfileListing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

}

public class Profile
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Picture { get; set; }
    public string Location { get; set; }
    public string Occupation { get; set; }
    public string Birthday { get; set; }
    public string Likes { get; set; }
    public string Dislikes { get; set; }
    public string Description { get; set; }
}

public class ProfileEvent
{
    public string Date { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
