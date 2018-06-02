using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
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
        Profile currentProfile;

        public MainWindow()
        {
            //Properties.Settings.Default.Reset();
            if (Properties.Settings.Default.ProfileList != null)
            {
                ProfileList = Properties.Settings.Default.ProfileList;
            }
            else
            {
                ProfileList = new List<Profile>();
            }

            InitializeComponent();
            RefreshProfileListing();

            Console.WriteLine(ProfileList.Count);
            Console.WriteLine(ProfileListing.Items.Count);
        }

        private void RefreshProfileListing()
        {
            ProfileListing.Items.Clear();
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
            ProfileForm newProfile = new ProfileForm();
            newProfile.Show();
        }

        public void AddProfile(Profile profile)
        {
            profile.Id = GetNextId();

            if (!string.IsNullOrEmpty(profile.Picture))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(profile.Picture);

                ResXResourceWriter rsxw = new ResXResourceWriter("profileImages.resx");
                rsxw.AddResource(profile.Id.ToString(), img);
                rsxw.Close();
            }

            ProfileList.Add(profile);
            Properties.Settings.Default.ProfileList = ProfileList;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Test = profile.Name;
            Properties.Settings.Default.Save();
            RefreshProfileListing();
        }

        public void UpdateProfile(Profile profile)
        {
            int index = ProfileList.FindLastIndex(p => p.Id == profile.Id);
            if (index != -1)
            {
                Console.WriteLine(ProfileList[index]);
                ProfileList[index] = profile;

                if (!string.IsNullOrEmpty(profile.Picture))
                {
                    System.Drawing.Image img = System.Drawing.Image.FromFile(profile.Picture);

                    ResXResourceWriter rsxw = new ResXResourceWriter("profileImages.resx");
                    rsxw.AddResource(profile.Id.ToString(), img);
                    rsxw.Close();
                }
            }
            RefreshProfileListing();
        }

        public void SetProfile(Profile profile)
        {
            WelcomeOverlay.Visibility = Visibility.Hidden;
            if (File.Exists("profileImages.resx"))
            {
                using (ResXResourceSet resxSet = new ResXResourceSet("profileImages.resx"))
                {
                    System.Drawing.Image img = (System.Drawing.Image)resxSet.GetObject(profile.Id.ToString(), true);
                    if (img != null)
                    {
                        using (MemoryStream memory = new MemoryStream())
                        {
                            img.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                            memory.Position = 0;
                            BitmapImage bitmapimage = new BitmapImage();
                            bitmapimage.BeginInit();
                            bitmapimage.StreamSource = memory;
                            bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapimage.EndInit();

                            ProfileImage.Source = bitmapimage;
                        }
                    }
                    else
                    {
                        ProfileImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/default.jpg"));
                    }
                }
            }
            else
            {
                ProfileImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/default.jpg"));
            }
            ProfileNumber.Text = "#" + profile.Id.ToString("D3");
            ProfileName.Text = profile.Name;
            ProfileLocation.Text = profile.Location;
            ProfileOccupation.Text = profile.Occupation;
            ProfileBirthday.Text = profile.Birthday;
            ProfileLikes.Text = profile.Likes;
            ProfileDislikes.Text = profile.Dislikes;
            ProfileDescription.Text = profile.Description;

            currentProfile = profile;
        }

        private int GetNextId()
        {
            return ProfileList.Count + 1;
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
            var selectedItems = ProfileListing.SelectedItems;
            foreach (Profile profile in selectedItems)
            {
                SetProfile(profile);
            }
        }

        private void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if(currentProfile != null)
            {
                ProfileForm editProfile = new ProfileForm(currentProfile);
                editProfile.Show();
            }
        }
    }

}

public class Profile
{
    public int Id { get; set; }
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
