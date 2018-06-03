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
        public EventForm(int Id)
        {
            InitializeComponent();
            IdInput.Text = Id.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Application.Current.MainWindow;
            ProfileEvent profileEvent = new ProfileEvent();
            profileEvent.Title = TitleInput.Text;
            profileEvent.Date = DateInput.Text;
            profileEvent.Description = DescriptionInput.Text;

            main.AddEvent(int.Parse(IdInput.Text), profileEvent);
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
