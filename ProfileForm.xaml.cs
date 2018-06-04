using Microsoft.Win32;
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
using System.Drawing;

namespace peopledex
{
    /// <summary>
    /// Interaction logic for ProfileForm.xaml
    /// </summary>
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
            }
            else
            {
                currentMode = FormMode.Edit;
                SubmitButton.Content = "Update";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateNewProfileForm())
            {
                MainWindow main = (MainWindow)Application.Current.MainWindow;
                Profile profile = new Profile();
                profile.Name = NameInput.Text;
                profile.Picture = PictureInput.Text;
                profile.Location = LocationInput.Text;
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

        private void PrepopulateForm(Profile profile)
        {
            IdInput.Text = profile.Id.ToString();
            NameInput.Text = profile.Name;
            PictureInput.Text = profile.Picture;
            LocationInput.Text = profile.Location;
            OccupationInput.Text = profile.Occupation;
            BirthdayInput.Text = profile.Birthday;
            LikeInput.Text = profile.Likes;
            DescriptionInput.Text = profile.Description;
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                //IResourceWriter writer = new ResourceWriter("myResources.resources");
                //Preview.Source = new BitmapImage(new Uri(op.FileName));
                //FileStream fs = File.Open(op.FileName, FileMode.Open);
                //writer.AddResource("dude", fs);

                PictureInput.Text = op.FileName;

                //System.Drawing.Image img = System.Drawing.Image.FromFile(op.FileName);
                //ResXResourceWriter rsxw = new ResXResourceWriter("en-AU.resx");
                //rsxw.AddResource("en-AU.jpg", img);
                //rsxw.Close();

            }
        }

        private void DeleteProfileButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Application.Current.MainWindow;
            main.DeleteProfile(int.Parse(IdInput.Text));
        }
    }
}
