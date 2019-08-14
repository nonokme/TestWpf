using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityTest.Interface;

namespace UnityTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //默认为多实例
            IBll x = Ioc.R<IBll>();
            x.GetStr();
            IBll x2 = Ioc.R<IBll>();
            x2.GetStr();
            var str = x == x2 ? "same" : "different";
            Console.WriteLine(string.Format("x and x2 is {0} object!", str));

            //单例
            IBll2 y = Ioc.R<IBll2>();
            y.GetStr("222");
            IBll2 y2 = Ioc.R<IBll2>();
            y2.GetStr("222");
            var str2 = y == y2 ? "same" : "different";
            Console.WriteLine(string.Format("y and y2 is {0} object!", str2));

            //结构方法带参数
            IBll3 z = Ioc.R<IBll3>();
            z.GetStr();

            //引用ServiceLocator模式
            var bll = ServiceLocator.Current.GetInstance<IBll3>();
            bll.GetStr();

            //依赖倒置
            IBll4 a = ServiceLocator.Current.GetInstance<IBll4>();
            a.GetStrBll4();
           
            Console.ReadKey();
        }
    }
}
