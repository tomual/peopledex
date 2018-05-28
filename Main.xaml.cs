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
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            InitializeProfile();

            ProfileListItem[] listing = new ProfileListItem[]
            {
                new ProfileListItem { Id = "2", Name = "Tom Olek" }
            };

            foreach (ProfileListItem profile in listing)
            {
                ProfileList.Items.Add(profile);
            }
        }

        private void InitializeProfile()
        {

            ProfileNumber.Text = "#013";
            ProfileName.Text = "Tomas Olek";
            ProfileImage.Source = new BitmapImage(new Uri(@"Resources\test.jpg", UriKind.Relative));

            ProfileLocation.Text = "Bellevue, NE";
            ProfileOccupation.Text = "Student at UNL";
            ProfileBirthday.Text = "23 March 1993";
            ProfileLikes.Text = "Cats, Cheese, Parks and Rec";
            ProfileDislikes.Text = "Chinese Food, Spiders, Candy";

            ProfileDescription.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio. Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum. Praesent mauris. Fusce nec tellus sed augue semper porta. Mauris massa. Vestibulum lacinia arcu eget nulla. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Curabitur sodales ligula in libero. Sed dignissim lacinia nunc. Curabitur tortor. Pellentesque nibh. Aenean quam. In scelerisque sem at dolor. Maecenas mattis. Sed convallis tristique sem. Proin ut ligula vel nunc egestas porttitor. Morbi lectus risus, iaculis vel, suscipit quis, luctus non, massa. Fusce ac turpis quis ligula lacinia aliquet. Mauris ipsum. Nulla metus metus, ullamcorper vel, tincidunt sed, euismod in, nibh. Quisque volutpat condimentum velit. ";

            ProfileEvent[] events = new ProfileEvent[]
            {
                new ProfileEvent { Date = "08/05/2018", Title = "Talked at Target", Description = "Talked to me about how his cat is dying. Cat's name is Hank." },
                new ProfileEvent { Date = "14/04/2018", Title = "Facebook chat", Description = "Moving to Switzerland in June for 15 months to study" },
                new ProfileEvent { Date = "01/04/2018", Title = "Lunch at Fazoli's", Description = "Brother (Troy) plays volleyball" }
            };

            foreach (ProfileEvent profileEvent in events)
            {
                ProfileEvents.Items.Add(profileEvent);
            }
        }
    }
}

public class ProfileListItem
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class ProfileEvent
{
    public string Date { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
