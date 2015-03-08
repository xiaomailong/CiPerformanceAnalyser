using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Visifire.Charts;
using System.Windows.Threading;

namespace CiPerformanceAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataReciever reciever;
        private Thread recv_thread;
        public int active_tab = 0;

        private delegate void DoTask();

        public MainWindow()
        {
            InitializeComponent();

            reciever = new DataReciever(this,3004);
            recv_thread = new Thread(new ThreadStart(Start));

            PrimarySeriesPrimaryCpu chart = new PrimarySeriesPrimaryCpu();
            Shower.Children.Add(chart);

            recv_thread.Start();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            recv_thread.Abort();
            reciever.Close();
        }
        private void Start()
        {
            while (true)
            {
                this.Dispatcher.Invoke(new DoTask(reciever.RecvAndDispatch));
            }
        }

        private void TabPrimarySeriesPrimaryCpu_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (active_tab == 0)
            {
                return;
            }
            Shower.Children.Clear();
            PrimarySeriesPrimaryCpu chart = new PrimarySeriesPrimaryCpu();
            Shower.Children.Add(chart);

            active_tab = 0;

            return;
        }

        private void TabPrimarySeriesSlaveCpu_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (active_tab == 1)
            {
                return;
            }
            Shower.Children.Clear();
            PrimarySeriesSlaveCpu chart = new PrimarySeriesSlaveCpu();
            Shower.Children.Add(chart);

            active_tab = 1;
            return;
        }

        private void TabSpareSeriesPrimaryCpu_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (active_tab == 2)
            {
                return;
            }
            Shower.Children.Clear();
            SpareSeriesPrimaryCpu chart = new SpareSeriesPrimaryCpu();
            Shower.Children.Add(chart);

            active_tab = 2;
            return;
        }

        private void TabSpareSeriesSlaveCpu_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (active_tab == 3)
            {
                return;
            }
            Shower.Children.Clear();
            SpareSeriesSlaveCpu chart = new SpareSeriesSlaveCpu();
            Shower.Children.Add(chart);

            active_tab = 3;
            return;
        }
    }
}