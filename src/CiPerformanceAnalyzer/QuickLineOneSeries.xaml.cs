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
    /// QuickLineOneSeries.xaml 的交互逻辑
    /// </summary>
    public partial class QuickLineOneSeries : UserControl
    {
        public QuickLineOneSeries()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(String), typeof(QuickLineOneSeries),
                                    new UIPropertyMetadata(String.Empty));

        public String Title
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty DataPointsProperty =
            DependencyProperty.Register("dataPoints", typeof (DataPointCollection), 
            typeof (QuickLineOneSeries), 
            new PropertyMetadata(default(DataPointCollection),DataPointsPropertyChangedCallBack));

        private static void DataPointsPropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPointCollection dataPoints = (DataPointCollection)d.GetValue(DataPointsProperty);
        }

        public DataPointCollection DataPoints
        {
            get { return (DataPointCollection) GetValue(DataPointsProperty); }
            set
            {
                SetValue(DataPointsProperty, value);
            }
        }

        public static readonly DependencyProperty AxisYMininumProperty =
            DependencyProperty.Register("AxisYMininum", typeof (double), typeof (QuickLineOneSeries),
            new PropertyMetadata(default(double)));

        public double AxisYMininum
        {
            get { return (double) GetValue(AxisYMininumProperty); }
            set { SetValue(AxisYMininumProperty, value); }
        }
    }
}
