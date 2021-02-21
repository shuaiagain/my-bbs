using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Base
{
    public class EnumeratorTest
    {
        #region Enumerator
        public EnumeratorTest()
        {

        }
        #endregion

        public void Output()
        {
            int[] arr = new int[] { 1, 2, 3, 4 };

            //获取枚举器
            IEnumerator enumArr = arr.GetEnumerator();

            while (enumArr.MoveNext())
            {
                int cur = (int)enumArr.Current;
                Console.WriteLine("{0}", cur);
            }
        }

    }
}
