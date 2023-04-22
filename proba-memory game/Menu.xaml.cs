using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static proba_memory_game.MainWindow;

namespace proba_memory_game
{
    public partial class Menu : Window
    {
        private string playerName;
        List<GameData> list;

        public Menu(string playerName)
        {
            InitializeComponent();
            this.playerName = playerName;
            list = new List<GameData>();
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            MemoryGame w = new MemoryGame(playerName);
            w.Show();
        }

        private void fillListOfGames()
        {
            list2.Visibility = Visibility.Visible;
            List<String> names = new List<String>();
            for (int i = 0; i < list.Count; i++)
            {
                names.Add(list[i].name);
            }
            list2.ItemsSource = null;
            list2.ItemsSource = names;
        }

        private void OpenGame_Click(object sender, RoutedEventArgs e)
        {
            string filename = $"{(string)playerName}.bin";

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                list = (List<GameData>)formatter.Deserialize(stream);
            }
            int count = list.Count;
            if(count==0)
                MessageBox.Show("You have 0 saved games.");
            else
                fillListOfGames();
        }

        private void list2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedGame = (string)list2.SelectedItem;

            GameData g = list.FirstOrDefault(u => u.name == selectedGame);
            MemoryGame w = new MemoryGame(g.data, g.data1,g.data2,g.lev,playerName,g.name);
            w.Show();
            
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
