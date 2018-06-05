using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace peopledex
{
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

        private void ClickCreateButton(object sender, RoutedEventArgs e)
        {
            if(ValidateNewProfileEventForm())
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
        }

        private bool ValidateNewProfileEventForm()
        {
            bool valid = true;
            List<TextBox> required = new List<TextBox>();
            required.Add(TitleInput);
            required.Add(DateInput);
            required.Add(DescriptionInput);

            foreach (TextBox requiredInput in required)
            {
                requiredInput.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFABADB3"));
                EventErrorLabel.Visibility = Visibility.Hidden;
                if (string.IsNullOrEmpty(requiredInput.Text))
                {
                    valid = false;
                    EventErrorLabel.Visibility = Visibility.Visible;
                    requiredInput.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#D26759"));
                }
            }

            return valid;
        }

        private void ClickCancelButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ClickDeleteEventButton(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Application.Current.MainWindow;
            main.DeleteProfileEvent(int.Parse(ProfileEventIdInput.Text));
            this.Close();
        }
    }
}
