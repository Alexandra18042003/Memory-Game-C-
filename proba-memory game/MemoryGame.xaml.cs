using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
using static proba_memory_game.MainWindow;


namespace proba_memory_game
{
    public partial class MemoryGame : Window
    {
        private Data data;
        private Data data1;
        private Data data2;
        private Card firstCard;
        private string playerName;
        private int finishedLevel = 0;
        private bool openGame=false;
        private string gameName;

        public MemoryGame(string playerName)
        {
            InitializeComponent();

            data = new Data();
            data.display = true;
            data1 = new Data();
            data2 = new Data();
            DataContext = data;

            this.playerName = playerName;

        }
        public MemoryGame(Data data,Data data1, Data data2,int level, string playerName, string gameName)
        {
            InitializeComponent();

            this.data = data;
            this.data1 = data1;
            this.data2 = data2;

            DataContext = data;

            data.display = true;
            if (level == 1)
                data1.display = true;
            if(level==2)
                data2.display = true;

            this.finishedLevel= level;
            this.playerName = playerName;

            openGame = true;
            this.gameName= gameName;
        }
        private bool checkCards_Left(ObservableCollection<List<Card>> cards)
        {
            foreach (var listCards in cards)
            {
                foreach (var card in listCards)
                {
                    if (!card.IsFlipped)
                        return true;
                }
            }
            return false;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var card = (Card)button.DataContext;

            if (card.IsFlipped)
            {
                return;
            }
            else
            {
                card.IsFlipped = true;
                card.UnflippedCardPath = card.FlippedCardPath;
                if (firstCard == null)
                {
                    firstCard = card;
                }
                else
                {
                    //daca ambele cards selectate au aceeasi imagine
                    if (firstCard.UnflippedCardPath == card.UnflippedCardPath)
                    {
                        await Task.Delay(500);
                        firstCard.UnflippedCardPath = "";
                        card.UnflippedCardPath = "";
                    }
                    else 
                    //daca sunt imagini diferite
                    {
                        firstCard.IsFlipped = false;
                        card.IsFlipped = false;
                        await Task.Delay(500);
                        firstCard.UnflippedCardPath = data.unflippedPath;
                        card.UnflippedCardPath = data.unflippedPath;
                    }
                    firstCard = null;
                }
            }
            if (!checkCards_Left(data.CardItems))
            {
                finishedLevel++;

                foreach (var listCards in data.CardItems)
                {
                    foreach (var c in listCards)
                    {
                        c.UnflippedCardPath = "D:\\FACULTATE\\AN 2 SEM 2\\MVP\\ARHIVA\\proba-memory game\\proba-memory game\\ImagesCards\\like.png";
                    }
                }

                if (data.display && !data1.display)
                {
                    data1.display = true;
                    await Task.Delay(3000);
                    data.CardItems = data1.CardItems;
                }
                else if (data.display && !data2.display)
                {
                    data2.display = true;
                    await Task.Delay(3000);
                    data.CardItems = data2.CardItems;
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            UserManager userManager= new UserManager();
            List<User> users = userManager.Deserialize("users.bin");

            foreach (var user in users)
            {
                if(user.name==playerName)
                {
                    user.playedGames++;
                    if (finishedLevel == 3)
                    {
                        user.wonGames++;
                        if(openGame==true)
                        {
                            List<GameData> list = new List<GameData>();
                            list = DeserializeGames();
                            for(int i=0;i<list.Count; i++)
                            {
                                if (list[i].name == gameName)
                                {
                                    list.Remove(list[i]);
                                    break;
                                }   
                            }
                            
                            string filename = $"{(string)playerName}.bin";
                            SerializeGames(list, filename);
                        }
                    }
                    break;
                }
            }
            userManager.Serialize(users, "users.bin");

            Close();
        }

        public List<GameData> DeserializeGames()
        {
            List<GameData> list = new List<GameData>();
            string filename = $"{(string)playerName}.bin";
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
               return (List<GameData>)formatter.Deserialize(stream);
            }
        }
        public void SerializeGames(List<GameData> list, string filename)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                formatter.Serialize(stream, list);
            }
        }
        private void SaveGame_Click(object sender, RoutedEventArgs e)
        {
            List<GameData> list = new List<GameData>();
            list = DeserializeGames();
            string filename = $"{(string)playerName}.bin";

            if (openGame == false)
            {
                string name = "game " + list.Count;
                GameData gameData = new GameData(data, data1, data2, finishedLevel, name);
                list.Add(gameData);
            }
            else if (openGame == true)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].name == gameName)
                    {
                        list[i].data = data;
                        list[i].data1 = data1;
                        list[i].data2 = data2;
                        list[i].lev = finishedLevel;
                    }
                }
            }
            SerializeGames(list, filename);

            MessageBox.Show("Saved the current game.");
            Close();
        }
    }
}
