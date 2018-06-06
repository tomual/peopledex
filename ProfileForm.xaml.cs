using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace peopledex
{
    public partial class ProfileForm : Window
    {
        enum FormMode { New, Edit };
        FormMode currentMode;

        public ProfileForm()
        {
            InitializeComponent();
            InitializeForm(FormMode.New);
        }

        public ProfileForm(Profile profile)
        {
            InitializeComponent();
            PrepopulateForm(profile);
            InitializeForm(FormMode.Edit);
        }

        private void InitializeForm(FormMode mode)
        {
            if (mode == FormMode.New)
            {
                currentMode = FormMode.New;
                DeleteProfileButton.Visibility = Visibility.Hidden;
                SubmitButton.Content = "Create";
                this.Title = "New Profile";
            }
            else
            {
                currentMode = FormMode.Edit;
                SubmitButton.Content = "Update";
                this.Title = "Edit Profile";
            }
        }

        private void PrepopulateForm(Profile profile)
        {
            IdInput.Text = profile.Id.ToString();
            NameInput.Text = profile.Name;
            PictureInput.Text = profile.Picture;
            LocationInput.Text = profile.Location;
            EmailInput.Text = profile.Email;
            OccupationInput.Text = profile.Occupation;
            BirthdayInput.Text = profile.Birthday;
            LikeInput.Text = profile.Likes;
            DescriptionInput.Text = profile.Description;
        }

        private void ClickCreateButton(object sender, RoutedEventArgs e)
        {
            if (ValidateNewProfileForm())
            {
                MainWindow main = (MainWindow)Application.Current.MainWindow;
                Profile profile = new Profile();
                profile.Name = NameInput.Text;
                profile.Picture = PictureInput.Text;
                profile.Location = LocationInput.Text;
                profile.Email = EmailInput.Text;
                profile.Occupation = OccupationInput.Text;
                profile.Birthday = BirthdayInput.Text;
                profile.Likes = LikeInput.Text;
                profile.Description = DescriptionInput.Text;

                if (currentMode == FormMode.New)
                {
                    main.AddProfile(profile);
                }
                else
                {
                    profile.Id = int.Parse(IdInput.Text);
                    main.UpdateProfile(profile);
                }

                main.SetProfile(profile);
                this.Close();
            }
        }

        private void ClickCancelButton(object sender, RoutedEventArgs e)
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

            foreach (TextBox requiredInput in required)
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

        private void ClickBrowseButton(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                PictureInput.Text = op.FileName;
            }
        }

        private void ClickDeleteProfileButton(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Application.Current.MainWindow;
            main.DeleteProfile(int.Parse(IdInput.Text));
            this.Close();
        }
    }
}
