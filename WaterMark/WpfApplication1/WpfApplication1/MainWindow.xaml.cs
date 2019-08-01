using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WpfApplication1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.pre1.Height = this.pre.Height;
            this.pre1.Width = this.pre.Width;

            for (int i = 20; i < this.pre1.Height; i += 80)
            {
                for (int j = 20; j < this.pre1.Width; j += 80)
                {

                    var txt = new TextBlock();
                    txt.Text = "127.0.0.1";
                    TransformGroup xx = new TransformGroup();
                    xx.Children.Add(new TranslateTransform(i, j));
                    //xx.Children.Add(new RotateTransform(-30));
                    txt.RenderTransformOrigin = new Point(0.5, 0.5);
                    txt.RenderTransform = xx;
                    this.pre1.Children.Add(txt);


                }
            }


        }
    }
}
