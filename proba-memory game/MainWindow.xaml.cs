using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace proba_memory_game
{
   
    public partial class MainWindow : Window
    {

        UserManager userManager;
        List<User> usersList;
        string playerName;
        //List<User> usersList = new List<User>()
        //{
        //    new User("Alexandra", "Images\\image4.png",0,0),
        //    new User("Gabriela", "Images\\image5.png",0,0),
        //    new User("Ioana", "Images\\image7.png",0,0),
        //    new User("Andrei", "Images\\image1.png",0,0),
        //    new User("Mihai", "Images\\image2.png",0,0),
        //    new User("Catalin", "Images\\image3.png",0,0),
        //    new User("Adrian", "Images\\image6.png",0,0)
        //};
        public MainWindow()
        {
            InitializeComponent();
            userManager = new UserManager();
            //userManager.Serialize(usersList, "users.bin");
            usersList = userManager.Deserialize("users.bin");
            fillListOfUsers();
        }

       
        [Serializable]
        public class User
        {
            public string name;
            public string imagePath;
            public int playedGames;
            public int wonGames;


            public User()
            {
            }

            public User(string v1, string v2, int v3, int v4)
            {
                this.name = v1;
                this.imagePath = v2;
                this.playedGames = v3;
                this.wonGames = v4;
            }
        }

        public class UserManager
        {
            BinaryFormatter formatter = new BinaryFormatter();

            public void Serialize(List<User> users, string fileName)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(fileName, FileMode.Create))
                {
                    formatter.Serialize(stream, users);
                }
            }

            public List<User> Deserialize(string fileName)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    return (List<User>)formatter.Deserialize(stream);
                }
            }
        }
        
        
        
        private void fillListOfUsers()
        {
            List<String> names = new List<String>();

            foreach (User user in usersList)
            {
                names.Add(user.name);
            }
            list1.ItemsSource = null;
            list1.ItemsSource = names;
        }

        private void list1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedUsername = (string)list1.SelectedItem;
            playerName = selectedUsername;

            User selectedUser = usersList.FirstOrDefault(u => u.name == selectedUsername);

            if (selectedUser != null)
            {
                string path = selectedUser.imagePath;
                myimage.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeleteUserBtn_Click(object sender, RoutedEventArgs e)
        {
            string selectedUsername = list1.SelectedItem.ToString();

            int index = usersList.FindIndex(user => user.name == selectedUsername);

            if (index != -1)
            {
                usersList.RemoveAt(index);
                userManager.Serialize(usersList, "users.bin");

                fillListOfUsers();
            }

            string filename = $"{(string)selectedUsername}.bin";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

        }

        private void NewUserBtn_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            user.name = txtName.Text;
            user.imagePath = usersList[int.Parse(chooseAvatar.Text) - 1].imagePath;
            user.playedGames = 0;
            user.wonGames = 0;

            usersList.Add(user);
            userManager.Serialize(usersList, "users.bin");

            txtName.Clear();
            chooseAvatar.Clear();
            fillListOfUsers();

            //creare fisier bin
            List<GameData> list = new List<GameData>();

            string filename = $"{(string)user.name}.bin";
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                formatter.Serialize(stream, list);
            }
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            userManager.Serialize(usersList, "users.bin");
            Menu w= new Menu(playerName);
            w.Show();
        }

        private void StatisticsBtn_Click(object sender, RoutedEventArgs e)
        {
            usersList = userManager.Deserialize("users.bin");
            User selectedUser = usersList.FirstOrDefault(u => u.name == this.playerName);

            MessageBox.Show("Nume utilizator - " + selectedUser.name + " - " + selectedUser.playedGames.ToString() + " - " + selectedUser.wonGames.ToString());

        }
    }
}
