using BaseTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BaseTest.Base
{
    public class ListTest
    {

        public static void Test()
        {
            List<TestModel> test = new List<TestModel>()
            {
                new TestModel() {name="aa",age=1 },
                new TestModel() {name="bb",age=2 }
            };

            int index = 0;
            foreach (var item in test)
            {
                Console.WriteLine(string.Format("{0} name:{1} ", ++index, item.name));
            }


            foreach (var item in test)
            {
                item.name = "abc";
            }

            index = 0;
            foreach (var item in test)
            {
                Console.WriteLine(string.Format("{0} name:{1} ", ++index, item.name));
            }


        }
    }
}
