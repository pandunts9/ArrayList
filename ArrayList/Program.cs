using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayList
{
    class Program
    {
        static void Main(string[] args)
        {
            _ArrayList ArrList = new _ArrayList();
            
            for (int i = 1; i < 15;i++)
            {
                ArrList.Add(i);
            }
            Console.WriteLine(ArrList[0]);
            ArrList.RemoveAt(ArrList.Count-1);
            ArrList.Remove(21);
            ArrList.RemoveRange(9, ArrList.Count-9);
            ArrList.Reverse(0, 3);
            foreach (var item in ArrList)
            {
                Console.WriteLine(item);
            }
        }
    }
}
