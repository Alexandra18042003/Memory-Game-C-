using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace proba_memory_game
{
    [Serializable]
    public class GameData
    {
        
        public Data data;
        public Data data1;
        public Data data2;
        public int lev;
        public string name;

        public GameData(Data data,Data data1,Data data2, int lev, string name)
        {
            this.data = data;
            this.data1 = data1;
            this.data2 = data2;
            this.lev = lev;
            this.name = name;
        }
    }
}
