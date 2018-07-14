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
            this.Title = "New Event";
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
            this.Title = "Edit Event";
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
            List<Control> required = new List<Control>();
            required.Add(TitleInput);
            required.Add(DateInput);
            required.Add(DescriptionInput);
            EventErrorLabel.Visibility = Visibility.Hidden;

            foreach (Control requiredInput in required)
            {
                requiredInput.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFABADB3"));
                if(requiredInput.GetType() == typeof(TextBox))
                {
                    if (string.IsNullOrEmpty(((TextBox)requiredInput).Text))
                    {
                        valid = false;
                        EventErrorLabel.Visibility = Visibility.Visible;
                        requiredInput.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#D26759"));
                    }
                } else if (requiredInput.GetType() == typeof(DatePicker)) {
                    if (string.IsNullOrEmpty(((DatePicker)requiredInput).ToString()))
                    {
                        valid = false;
                        EventErrorLabel.Visibility = Visibility.Visible;
                        requiredInput.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#D26759"));
                    }
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
