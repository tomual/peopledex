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

            if(ProfileList.Count > 0)
            {
                currentProfile = ProfileList[ProfileList.Count - 1];
            }
        }

        private void RefreshProfileListing()
        {
            ProfileListing.Items.Clear();
            if (Properties.Settings.Default.ProfileList != null)
            {
                foreach (Profile profile in ProfileList)
                {
                    ProfileListing.Items.Add(profile);
                }
            }
        }

        private void RefreshProfileEventsListing()
        {
            ProfileEventsListing.Items.Clear();
            if(currentProfile.ProfileEvents != null)
            {
                foreach (ProfileEvent profileEvent in currentProfile.ProfileEvents)
                {
                    ProfileEventsListing.Items.Add(profileEvent);
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
            profile.Id = GetNextProfileId();

            if (!string.IsNullOrEmpty(profile.Picture))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(profile.Picture);

                ResXResourceWriter rsxw = new ResXResourceWriter("profileImages.resx");
                rsxw.AddResource(profile.Id.ToString(), img);
                rsxw.Close();
            }

            ProfileList.Add(profile);
            Properties.Settings.Default.Id = ProfileList.Count + 1;
            SaveToStorage();
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
            SaveToStorage();
            RefreshProfileListing();
        }

        private void SaveToStorage()
        {
            Properties.Settings.Default.ProfileList = ProfileList;
            Properties.Settings.Default.Save();
        }

        public void UpdateProfileEvent(ProfileEvent profileEvent)
        {
            int index = currentProfile.ProfileEvents.FindLastIndex(p => p.Id == profileEvent.Id);
            if (index != -1)
            {
                Console.WriteLine(ProfileList[index]);
                currentProfile.ProfileEvents[index] = profileEvent;
                UpdateProfile(currentProfile);
            }
            RefreshProfileListing();
            SetProfile(currentProfile);
        }

        public void DeleteProfile(int Id)
        {
            int index = ProfileList.FindLastIndex(p => p.Id == Id);
            if (index != -1)
            {
                Console.WriteLine(ProfileList[index]);
                ProfileList.RemoveAt(index);

            }
            SaveToStorage();
            RefreshProfileListing();
        }

        public void DeleteProfileEvent(int Id)
        {
            int index = currentProfile.ProfileEvents.FindLastIndex(p => p.Id == Id);
            if (index != -1)
            {
                currentProfile.ProfileEvents.RemoveAt(index);
                UpdateProfile(currentProfile);
                SaveToStorage();
            }
            SetProfile(currentProfile);
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
            ProfileSubtext.Text = profile.Location;
            if(!string.IsNullOrEmpty(profile.Email))
            {
                ProfileSubtext.Text += "  ·  " + profile.Email;
            }
            ProfileOccupation.Text = profile.Occupation;
            ProfileBirthday.Text = profile.Birthday;
            ProfileLikes.Text = profile.Likes;
            ProfileDescription.Text = profile.Description;

            currentProfile = profile;
            RefreshProfileEventsListing();
        }

        public void AddEvent(int Id, ProfileEvent profileEvent)
        {
            int index = ProfileList.FindLastIndex(p => p.Id == Id);
            if (index != -1)
            {
                profileEvent.Id = GetNextProfileEventId();
                if(ProfileList[index].ProfileEvents == null)
                {
                    ProfileList[index].ProfileEvents = new List<ProfileEvent>();
                }
                ProfileList[index].ProfileEvents.Add(profileEvent);
            }
            UpdateProfile(ProfileList[index]);
            SetProfile(ProfileList[index]);
            Console.WriteLine(profileEvent.Id);
        }

        private int GetNextProfileId()
        {
            return Properties.Settings.Default.Id;
        }

        private int GetNextProfileEventId()
        {
            if(currentProfile.ProfileEvents != null && currentProfile.ProfileEvents.Count > 0 )
            {
                int index = currentProfile.ProfileEvents.Count - 1;
                return currentProfile.ProfileEvents[index].Id + 1;
            }
            return 1;
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
            if (currentProfile != null)
            {
                ProfileForm editProfile = new ProfileForm(currentProfile);
                editProfile.Show();
            }
        }

        private void NewEventButton_Click(object sender, RoutedEventArgs e)
        {
            EventForm newEvent = new EventForm(currentProfile.Id);
            newEvent.Show();
        }

        private void ProfileEventsListing_DoubleClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = ProfileEventsListing.SelectedItems;
            foreach (ProfileEvent profileEvent in selectedItems)
            {
                EventForm editEvent = new EventForm(currentProfile.Id, profileEvent);
                editEvent.Show();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(SearchInput.Text))
            {
                ProfileListing.Items.Clear();
                if (Properties.Settings.Default.ProfileList != null)
                {
                    foreach (Profile profile in ProfileList)
                    {
                        if(profile.Name.ToLower().Contains(SearchInput.Text.ToLower()))
                        {
                            ProfileListing.Items.Add(profile);
                        }
                    }
                }
            }
            else
            {
                RefreshProfileListing();
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
    public string Email { get; set; }
    public string Occupation { get; set; }
    public string Birthday { get; set; }
    public string Likes { get; set; }
    public string Description { get; set; }
    public List<ProfileEvent> ProfileEvents { get; set; }
}

public class ProfileEvent
{
    public int Id { get; set; }
    public string Date { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
