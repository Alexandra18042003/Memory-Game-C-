using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proba_memory_game
{
    [Serializable]
    public class Card : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public string flippedCardPath { get; set; }
        public string unflippedCardPath { get; set; }
        public bool isFlipped { get; set; }
        public string FlippedCardPath
        {
            get { return flippedCardPath; }
            set { flippedCardPath = value; OnPropertyChanged(nameof(FlippedCardPath)); }
        }
        public string UnflippedCardPath
        {
            get { return unflippedCardPath; }
            set { unflippedCardPath = value; OnPropertyChanged(nameof(UnflippedCardPath)); }
        }

        public bool IsFlipped
        {
            get { return isFlipped; }
            set { isFlipped = value; OnPropertyChanged(nameof(IsFlipped)); }
        }

        public Card(string path, string unflippedCardPath)
        {
            this.flippedCardPath = path;
            this.IsFlipped = false;
            this.unflippedCardPath = unflippedCardPath;
        }
        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
   
}
