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
    /// Interaction logic for NewProfile.xaml
    /// </summary>
    public partial class NewProfile : Window
    {
        public NewProfile()
        {
            InitializeComponent();

            //// Create a ResXResourceReader for the file items.resx.
            //ResXResourceReader rsxr = new ResXResourceReader("en-AU.resx");

            //// Create an IDictionaryEnumerator to iterate through the resources.
            //IDictionaryEnumerator id = rsxr.GetEnumerator();

            //// Iterate through the resources and display the contents to the console.
            //foreach (DictionaryEntry d in rsxr)
            //{
            //    Console.WriteLine(d.Key.ToString() + ":\t" + d.Value.ToString());

            //    System.Drawing.Image img = (System.Drawing.Image)d.Value;
            //    using (MemoryStream memory = new MemoryStream())
            //    {
            //        img.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
            //        memory.Position = 0;
            //        BitmapImage bitmapimage = new BitmapImage();
            //        bitmapimage.BeginInit();
            //        bitmapimage.StreamSource = memory;
            //        bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
            //        bitmapimage.EndInit();

            //        Preview.Source = bitmapimage;
            //    }

            //}

            ////Close the reader.
            //rsxr.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateNewProfileForm())
            {
                Profile profile = new Profile();
                profile.Id = 1;
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
    }
}
