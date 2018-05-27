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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace peopledex
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ProfileEvent[] events = new ProfileEvent[]
            {
                new ProfileEvent { Date = "08/05/2018", Label = "Talked at Target", Description = "Talked to me about how his cat is dying. Cat's name is Hank." },
                new ProfileEvent { Date = "14/04/2018", Label = "Facebook chat", Description = "Moving to Switzerland in June for 15 months to study" },
                new ProfileEvent { Date = "01/04/2018", Label = "Lunch at Fazoli's", Description = "Brother (Troy) plays volleyball" }
            };

            foreach (ProfileEvent profileEvent in events)
            {
                ProfileEvents.Items.Add(profileEvent);

            }

            ProfileAccount[] accounts = new ProfileAccount[]
            {
                new ProfileAccount { Type = "Instagram", Name = "churrochurro88" },
                new ProfileAccount { Type = "Facebook", Name = "https://www.facebook.com/tina.turner.4" },
                new ProfileAccount { Type = "Website", Name = "https://rainydevil.net" }
            };

            foreach (ProfileAccount profileAccount in accounts)
            {
                ProfileAccounts.Items.Add(profileAccount);

            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

public class ProfileEvent
{
    public string Date { get; set; }
    public string Label { get; set; }
    public string Description { get; set; }
}

public class ProfileAccount
{
    public string Type { get; set; }
    public string Name { get; set; }
}