using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace proba_memory_game
{
    [Serializable]
    public class Data : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


        private ObservableCollection<List<Card>> cardItems;
        public bool display = false;
        public string unflippedPath = "D:\\FACULTATE\\AN 2 SEM 2\\MVP\\ARHIVA\\proba-memory game\\proba-memory game\\ImagesCards\\daisy.png";
        
        public ObservableCollection<List<Card>> CardItems
        {
            get { return cardItems; }
            set { cardItems = value; OnPropertyChanged("CardItems"); }
        }
        public Data()
        {
            CardItems = new ObservableCollection<List<Card>>();
            var usedNumbers = new Dictionary<int, int>();
            var random = new Random();

            for (int i = 0; i < 5; i++)
            {
                List<Card> item = new List<Card>();
                for (int j = 0; j < 5; j++)
                {
                    if (i == 4 && j == 4)
                    {
                        item.Add(new Card("", ""));
                        item.Last().IsFlipped = true;
                    }
                    else
                        item.Add(new Card("D:\\FACULTATE\\AN 2 SEM 2\\MVP\\proba-memory game2\\proba-memory game\\proba-memory game\\ImagesFlowers\\Flowers\\flower" + GenerateUniqueRandomNumber(usedNumbers, random) + ".png",
                            unflippedPath));
                }

                Random rng = new Random();

                int n = item.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    var temp = item[k];
                    item[k] = item[n];
                    item[n] = temp;
                }
                CardItems.Add(item);
            }
        }

        static int GenerateUniqueRandomNumber(Dictionary<int, int> usedNumbers, Random random)
        {
            int number;
            do
            {
                number = random.Next(1, 13);
            } while (usedNumbers.ContainsKey(number) && usedNumbers[number] >= 2);

            if (usedNumbers.ContainsKey(number))
            {
                usedNumbers[number]++;
            }
            else
            {
                usedNumbers[number] = 1;
            }

            return number;
        }

        
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
