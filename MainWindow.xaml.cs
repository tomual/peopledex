using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace peopledex
{
    public partial class MainWindow : Window
    {
        List<Profile> ProfileList;
        Profile currentProfile;

        public MainWindow()
        {
            // Load profile listing from storage
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
        }

        // Save profile list to storage
        private void SaveToStorage()
        {
            Properties.Settings.Default.ProfileList = ProfileList;
            Properties.Settings.Default.Save();
        }

        // Refresh profiles ListView
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

        // Refresh profile's events ListView
        private void RefreshProfileEventsListing()
        {
            ProfileEventsListing.Items.Clear();
            if (currentProfile.ProfileEvents != null)
            {
                foreach (ProfileEvent profileEvent in currentProfile.ProfileEvents)
                {
                    ProfileEventsListing.Items.Add(profileEvent);
                }
            }
        }

        // Add Profile
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

        // Update profile
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

        // Update profile event
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

        // Delete profile
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
            WelcomeOverlay.Visibility = Visibility.Visible;
        }

        // Delete profile event
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

        // Set currently visible profile
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
            if (!string.IsNullOrEmpty(profile.Email))
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

        // Add profile event
        public void AddEvent(int Id, ProfileEvent profileEvent)
        {
            int index = ProfileList.FindLastIndex(p => p.Id == Id);
            if (index != -1)
            {
                profileEvent.Id = GetNextProfileEventId();
                if (ProfileList[index].ProfileEvents == null)
                {
                    ProfileList[index].ProfileEvents = new List<ProfileEvent>();
                }
                ProfileList[index].ProfileEvents.Add(profileEvent);
            }
            UpdateProfile(ProfileList[index]);
            SetProfile(ProfileList[index]);
            Console.WriteLine(profileEvent.Id);
        }

        // Get next ID # for profile
        private int GetNextProfileId()
        {
            return Properties.Settings.Default.Id;
        }

        // Get next ID # for profile's events
        private int GetNextProfileEventId()
        {
            if (currentProfile.ProfileEvents != null && currentProfile.ProfileEvents.Count > 0)
            {
                int index = currentProfile.ProfileEvents.Count - 1;
                return currentProfile.ProfileEvents[index].Id + 1;
            }
            return 1;
        }

        // Event - Change profile selection
        private void ChangedProfileListing(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = ProfileListing.SelectedItems;
            foreach (Profile profile in selectedItems)
            {
                SetProfile(profile);
            }
        }

        // Event - Click new profile button
        private void ClickNewProfileButton(object sender, RoutedEventArgs e)
        {
            ProfileForm newProfile = new ProfileForm();
            newProfile.Show();
        }

        // Event - Click edit profile button
        private void ClickEditProfileButton(object sender, RoutedEventArgs e)
        {
            if (currentProfile != null)
            {
                ProfileForm editProfile = new ProfileForm(currentProfile);
                editProfile.Show();
            }
        }

        // Event - Click new event button on profile
        private void ClickNewEventButton(object sender, RoutedEventArgs e)
        {
            EventForm newEvent = new EventForm(currentProfile.Id);
            newEvent.Show();
        }

        // Event - Double Click on profile event
        private void ProfileEventsListing_DoubleClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = ProfileEventsListing.SelectedItems;
            foreach (ProfileEvent profileEvent in selectedItems)
            {
                EventForm editEvent = new EventForm(currentProfile.Id, profileEvent);
                editEvent.Show();
            }
        }

        // Event - Click on search button
        private void ClickSearchButton(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchInput.Text))
            {
                ProfileListing.Items.Clear();
                if (Properties.Settings.Default.ProfileList != null)
                {
                    foreach (Profile profile in ProfileList)
                    {
                        if (profile.Name.ToLower().Contains(SearchInput.Text.ToLower()))
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

        // Event - Click next profile button
        private void ClickNextProfileButton(object sender, RoutedEventArgs e)
        {
            int index = ProfileList.FindLastIndex(p => p.Id == currentProfile.Id);
            if (index != -1)
            {
                if (index != ProfileList.Count - 1)
                {
                    SetProfile(ProfileList[index + 1]);
                    ProfileListing.SelectedItem = ProfileListing.Items[index + 1];
                }
            }
        }

        // Event - Click previous profile button
        private void ClickPreviousProfileButton(object sender, RoutedEventArgs e)
        {
            int index = ProfileList.FindLastIndex(p => p.Id == currentProfile.Id);
            if (index != -1)
            {
                if (index != 0)
                {
                    SetProfile(ProfileList[index - 1]);
                    ProfileListing.SelectedItem = ProfileListing.Items[index - 1];
                }
            }
        }
    }
}
