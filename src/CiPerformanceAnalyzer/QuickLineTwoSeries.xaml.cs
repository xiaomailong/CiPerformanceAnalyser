using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace CiPerformanceAnalyzer
{
    /// <summary>
    /// QuickLineTwoSeries.xaml 的交互逻辑
    /// </summary>
    public partial class QuickLineTwoSeries : UserControl
    {
        public QuickLineTwoSeries()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(String), typeof(QuickLineTwoSeries),
                                    new UIPropertyMetadata(String.Empty));

        public String Title
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty DataPointsSendProperty =
            DependencyProperty.Register("DataPointsSend", typeof (DataPointCollection),
            typeof (QuickLineTwoSeries), new PropertyMetadata(default(DataPointCollection)));

        public DataPointCollection DataPointsSend
        {
            get { return (DataPointCollection) GetValue(DataPointsSendProperty); }
            set { SetValue(DataPointsSendProperty, value); }
        }

        public static readonly DependencyProperty DataPointsRecvProperty =
            DependencyProperty.Register("DataPointsRecv", typeof (DataPointCollection),
            typeof (QuickLineTwoSeries), new PropertyMetadata(default(DataPointCollection)));

        public DataPointCollection DataPointsRecv
        {
            get { return (DataPointCollection) GetValue(DataPointsRecvProperty); }
            set { SetValue(DataPointsRecvProperty, value); }
        }
    }
}
