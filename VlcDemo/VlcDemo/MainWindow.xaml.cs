using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Reflection;
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

namespace VlcDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DirectoryInfo _vlcLibPath;
        private double videoLength;

        public MainWindow()
        {
            InitializeComponent();

            this.InitialVlcControl();
            this.Closing += MainWindow_Closing;
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.vlcControl.SourceProvider.MediaPlayer.IsPlaying()
                    || this.vlcControl.SourceProvider.MediaPlayer.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Stopped
                    || this.vlcControl.SourceProvider.MediaPlayer.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Paused)
            {
                try
                {
                    this.vlcControl.SourceProvider.MediaPlayer.Stop();
                    this.vlcControl.SourceProvider.MediaPlayer.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void InitialVlcControl()
        {
            try
            {
                var currentAssembly = Assembly.GetEntryAssembly();
                var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
                if (currentDirectory != null)
                    _vlcLibPath = new DirectoryInfo(Path.Combine(currentDirectory, "VLCLib", "x86"));

                vlcControl.SourceProvider.CreatePlayer(_vlcLibPath);
                vlcControl.SourceProvider.MediaPlayer.SnapshotTaken += MediaPlayer_SnapshotTaken;
                vlcControl.SourceProvider.MediaPlayer.Stopped += MediaPlayer_Stopped;
                //vlcControl.SourceProvider.MediaPlayer.LengthChanged += MediaPlayer_LengthChanged;
                //vlcControl.SourceProvider.MediaPlayer.PositionChanged += MediaPlayer_PositionChanged;

                this.timeSlider.Minimum = 0;
                this.timeSlider.Interval = 50;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void MediaPlayer_PositionChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.timeSlider.Value = e.NewPosition * this.videoLength;
                Console.WriteLine(e.NewPosition);
            });
        }

        void MediaPlayer_LengthChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerLengthChangedEventArgs e)
        {
            this.videoLength = e.NewLength;
            var total = TimeSpan.FromMilliseconds(e.NewLength).ToString("hh\\:mm\\:ss");
            Console.WriteLine(total);
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.timeSlider.Maximum = e.NewLength;
            });
        }

        void MediaPlayer_Stopped(object sender, Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs e)
        {
            
        }

        void MediaPlayer_SnapshotTaken(object sender, Vlc.DotNet.Core.VlcMediaPlayerSnapshotTakenEventArgs e)
        {
            this.ScreenshotWithWatermark(e.FileName, "10.161.66.199", Brushes.White, 3, 3);
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string url = string.Empty;
                if (string.IsNullOrEmpty(this.tbUrl.Text))
                {
                    url = @"http://10.161.64.22:31502/mrps-streamserver/mediaPlay/mediafileLocal/QzpcUFJPR1JBfjJcSHl0ZXJhXE1SUFNcUmVjb3JkXFJFUExBWX4xXFRtcEhMU1wyMDE5MDcyMjA5NDEzM196ZGZfMzAuMS4wLjIxM18yMDIyMFw=/20190722094133_zdf_30.1.0.213_20220.m3u8";
                }
                //this.vlcControl.SourceProvider.MediaPlayer.Play(url);

                this.vlcControl.SourceProvider.MediaPlayer.Play(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Video\\Wildlife.wmv")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btnSnapshot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = string.Format(@"D:\Snapshot\{0}.png", DateTime.Now.ToString("yyyy-MM-dd_HHmmss"));
                FileInfo file = new FileInfo(filePath);

                bool success = this.vlcControl.SourceProvider.MediaPlayer.TakeSnapshot(file);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 合成带水印的图片
        /// </summary>
        /// <param name="filePath">原始图片全路径</param>
        /// <param name="watermark">水印内容</param>
        /// <param name="foreground">水印颜色</param>
        /// <param name="rows">水印显示的行数</param>
        /// <param name="columns">水印显示的列数</param>
        /// <param name="fontSize">水印大小</param>
        /// <param name="opacity">水印透明度</param>
        /// <param name="angle">水印显示角度</param>
        private void ScreenshotWithWatermark(string filePath, string watermark, Brush foreground, int rows = 3, int columns = 3
            , double fontSize = 22, double opacity = 0.3, double angle = -20)
        {
            BitmapImage bgImage;

            //从流中读取图片，防止出现资源被占用的问题
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                FileInfo fi = new FileInfo(filePath);
                byte[] bytes = reader.ReadBytes((int)fi.Length);

                bgImage = new BitmapImage();
                bgImage.BeginInit();
                bgImage.StreamSource = new MemoryStream(bytes);
                bgImage.EndInit();
                bgImage.CacheOption = BitmapCacheOption.OnLoad;
            }

            RenderTargetBitmap composeImage = new RenderTargetBitmap(bgImage.PixelWidth, bgImage.PixelHeight, bgImage.DpiX, bgImage.DpiY, PixelFormats.Default);
            FormattedText signatureTxt = new FormattedText(watermark, CultureInfo.CurrentCulture, FlowDirection.LeftToRight
                , new Typeface(SystemFonts.MessageFontFamily, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), fontSize, foreground);

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawImage(bgImage, new Rect(0, 0, bgImage.Width, bgImage.Height));

            #region 设置水印的旋转角度及透明度
            //x，y为水印旋转的中心点
            double centerX = (bgImage.Width - signatureTxt.Width) / 2;
            double centerY = (bgImage.Height - signatureTxt.Height) / 2;

            drawingContext.PushOpacity(0.3);
            drawingContext.PushTransform(new RotateTransform(-20, centerX, centerY));
            #endregion

            #region 绘制全屏水印
            double intervalX = bgImage.Width / columns; //水印水平间隔
            double intervalY = bgImage.Height / rows; //水印垂直间隔

            for (double i = 0; i < bgImage.Width; i += intervalX)
            {
                for (double j = 0; j < bgImage.Height + intervalY; j += intervalY)
                {
                    if ((j / intervalY) % 2 == 0)
                    {
                        drawingContext.DrawText(signatureTxt, new Point(i, j));
                    }
                    else
                    {
                        drawingContext.DrawText(signatureTxt, new Point(i + intervalX / 2, j));
                    }
                }
            }
            #endregion

            drawingContext.Close();

            composeImage.Render(drawingVisual);

            PngBitmapEncoder bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(composeImage));

            //删除VLC生成的原始截图
            File.Delete(filePath);

            //将合成水印的截图保存到本地
            using (Stream stream = File.OpenWrite(filePath))
            {
                bitmapEncoder.Save(stream);
            }
            bgImage = null;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.vlcControl.SourceProvider.MediaPlayer.Stop();
            });
        }
    }
}
