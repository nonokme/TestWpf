using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityTest.Interface;

namespace UnityTest.BllClass
{
    public class Bll : IBll
    {
        public void GetStr()
        {
            Console.WriteLine("this is 111...");
        }
    }

    public class Bll2 : IBll2
    {
        public void GetStr(string a)
        {
            Console.WriteLine(string.Format("this is {0}...", a));
        }
    }

    public class Bll3 : IBll3
    {
        private string b;

        public Bll3(string aa)
        {
            b = aa;
        }
        public void GetStr()
        {
            Console.WriteLine(string.Format("this is {0}...", b));
        }
    }

    public class DAL : IDAL
    {
        public void GetStr()
        {
            Console.WriteLine("this is DAL111...");
        }
    }

    public class Bll4 : IBll4
    {
        private IDAL _DAL;
        public Bll4(IDAL dal)
        {
            _DAL = dal;
        }

        public void GetStrBll4()
        {
            _DAL.GetStr();
        }
    }
}
