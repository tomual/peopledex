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
    /// Interaction logic for EventForm.xaml
    /// </summary>
    public partial class EventForm : Window
    {
        enum FormMode { New, Edit };
        FormMode currentMode;

        public EventForm(int id)
        {
            InitializeComponent();
            ProfileIdInput.Text = id.ToString();
            InitializeForm(FormMode.New);
        }

        public EventForm(int id, ProfileEvent profileEvent)
        {
            InitializeComponent();
            InitializeForm(FormMode.Edit);
            ProfileIdInput.Text = id.ToString();
            ProfileEventIdInput.Text = profileEvent.Id.ToString();
            TitleInput.Text = profileEvent.Title;
            DateInput.Text = profileEvent.Date;
            DescriptionInput.Text = profileEvent.Description;
        }

        private void InitializeForm(FormMode mode)
        {
            if (mode == FormMode.New)
            {
                currentMode = FormMode.New;
                DeleteEventButton.Visibility = Visibility.Hidden;
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
            MainWindow main = (MainWindow)Application.Current.MainWindow;
            ProfileEvent profileEvent = new ProfileEvent();
            profileEvent.Title = TitleInput.Text;
            profileEvent.Date = DateInput.Text;
            profileEvent.Description = DescriptionInput.Text;

            if (currentMode == FormMode.New)
            {
                main.AddEvent(int.Parse(ProfileIdInput.Text), profileEvent);
            }
            else
            {
                profileEvent.Id = int.Parse(ProfileEventIdInput.Text);
                main.UpdateProfileEvent(profileEvent);
            }

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteEventButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
