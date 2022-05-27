using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;
namespace BSC_Stand.ViewModels
{
    internal class RealTimeGraphsViewModel:ViewModels.Base.ViewModelBase
    {
        public PlotModel PlotModel1 { get; set; }
        public PlotModel PlotModel2 { get; set; }
        public PlotModel PlotModel3 { get; set; }


        public int _SelectedGraphIndex;
        public int SelectedGraphIndex {
            get => _SelectedGraphIndex;
            set=> Set(ref _SelectedGraphIndex, value);
        }
        #region LineSeries
        public LineSeries V27Series { get;  set; }
        public LineSeries I27Series { get;  set; }
        public LineSeries V100Series { get;  set; }
        public LineSeries I100Series { get;  set; }
        public LineSeries TIBXASeries { get;  set; }
        public LineSeries TBSCSeries { get; set; }
        public LineSeries ITCVSeries { get;  set; }
        public LineSeries ITCASeries { get; set; }
        public LineSeries ITCWSeries { get;  set; }
        public LineSeries AKIPVSeries { get;  set; }
        public LineSeries AKIPASeries { get;  set; }
        public LineSeries AKIPWSeries { get;  set; }
        #endregion
        #region GraphVisible

        private bool _AKIPVSeriesVisible;
        public bool AKIPVSeriesVisible
        {
            get
            {
                return _AKIPVSeriesVisible;
            }
            set
            {
                Set(ref _AKIPVSeriesVisible, value);
                AKIPVSeries.IsVisible = value;
                PlotModel1.InvalidatePlot(true);
            }
        }

        private bool _AKIPASeriesVisible;
        public bool AKIPASeriesVisible
        {
            get
            {
                return _AKIPASeriesVisible;
            }
            set
            {
                Set(ref _AKIPASeriesVisible, value);
                AKIPASeries.IsVisible = value;
                PlotModel1.InvalidatePlot(true);
            }
        }

        private bool _AKIPWSeriesVisible;
        public bool AKIPWSeriesVisible
        {
            get
            {
                return _AKIPWSeriesVisible;
            }
            set
            {
                Set(ref _AKIPWSeriesVisible, value);
                AKIPWSeries.IsVisible = value;
                PlotModel1.InvalidatePlot(true);
            }
        }

        private bool _ITCVSeriesVisible;
        public bool ITCVSeriesVisible
        {
            get
            {
                return _ITCVSeriesVisible;
            }
            set
            {
                Set(ref _ITCVSeriesVisible, value);
                ITCVSeries.IsVisible = value;
                PlotModel1.InvalidatePlot(true);
            }
        }

        private bool _ITCASeriesVisible;
        public bool ITCASeriesVisible
        {
            get
            {
                return _ITCASeriesVisible;
            }
            set
            {
                Set(ref _ITCASeriesVisible, value);
                ITCASeries.IsVisible = value;
                PlotModel1.InvalidatePlot(true);
            }
        }
        private bool _ITCWSeriesVisible;
        public bool ITCWSeriesVisible
        {
            get
            {
                return _ITCWSeriesVisible;
            }
            set
            {
                Set(ref _ITCWSeriesVisible, value);
                ITCWSeries.IsVisible = value;
                PlotModel1.InvalidatePlot(true);
            }
        }


        private bool _V100SeriesVisible;
        public bool V100SeriesVisible
        {
            get
            {
                return _V100SeriesVisible;
            }
            set
            {
                Set(ref _V100SeriesVisible, value);
                V100Series.IsVisible = value;
                PlotModel1.InvalidatePlot(true);
            }
        }

        private bool _I100SeriesVisible;
        public bool I100SeriesVisible
        {
            get
            {
                return _I100SeriesVisible;
            }
            set
            {
                Set(ref _I100SeriesVisible, value);
                I100Series.IsVisible = value;
                PlotModel1.InvalidatePlot(true);
            }
        }

        private bool _I27SeriesVisible;
        public bool I27SeriesVisible
        {
            get
            {
                return _I27SeriesVisible;
            }
            set
            {
                Set(ref _I27SeriesVisible, value);
                I27Series.IsVisible = value;
                PlotModel1.InvalidatePlot(true);
            }
        }


        private bool _V27SeriesVisible;
        public bool V27SeriesVisible
        {
            get
            {
                return _V27SeriesVisible;
            }
            set
            {
                Set(ref _V27SeriesVisible, value);
                V27Series.IsVisible = value;
                PlotModel1.InvalidatePlot(true);
            }
        }


        private bool _TIBXASeriesVisible;
        public bool TIBXASeriesVisible
        {
            get
            {
                return _TIBXASeriesVisible;
            }
            set
            {
                Set(ref _TIBXASeriesVisible, value);
                TIBXASeries.IsVisible = value;
                PlotModel1.InvalidatePlot(true);
            }
        }

        private bool _TBSCSeriesVisible;
        public bool TBSCSeriesVisible
        {
            get
            {
                return _TBSCSeriesVisible;
            }
            set
            {

                Set(ref _TBSCSeriesVisible, value);
                TBSCSeries.IsVisible = value;
                PlotModel1.InvalidatePlot(true);
            }
        }




        #endregion
        public RealTimeGraphsViewModel()
        {
            InitGraphSeries();
        }





        private void InitGraphSeries()
        {
            PlotModel1 = new PlotModel();
            PlotModel1.Series.Add(new LineSeries()
            {

            });

            PlotModel2 = new PlotModel();
            PlotModel2.Series.Add(new LineSeries()
            {

            });

            PlotModel3 = new PlotModel();
            PlotModel3.Series.Add(new LineSeries()
            {

            });

            V27Series = new LineSeries
            {
                Title = "V 27",
                TrackerFormatString = "{4:0.###} В {2:0.##} сек",
                Color = OxyColors.Blue,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true
            };



            I27Series = new LineSeries
            {
                Title = "I 27",
                TrackerFormatString = "{4:0.###} A {2:0.##} сек",
                Color = OxyColors.BlueViolet,
                MarkerFill = OxyColors.DarkBlue,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true

            };


            V100Series = new LineSeries
            {
                Title = "V 100",
                TrackerFormatString = "{4:0} В {2:0} сек",
                Color = OxyColors.DarkOrange,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true,
            };



            I100Series = new LineSeries
            {
                Title = "I 100",
                TrackerFormatString = "{4:0} A {2:0} сек",
                Color = OxyColors.OrangeRed,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true,
            };


            TIBXASeries = new LineSeries
            {
                Title = "T℃  ИБХА",
                TrackerFormatString = "{4:0} T℃  {2:0} сек {0}",
                Color = OxyColors.Green,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true,
            };

            TBSCSeries = new LineSeries
            {
                Title = "T℃  ЭОБСК",
                TrackerFormatString = "{4:0} T℃  {2:0} сек {0}",
                Color = OxyColors.ForestGreen,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true,
            };



            ITCVSeries = new LineSeries
            {
                Title = "V IT8516C+",
                TrackerFormatString = "{4:0.###} В {2:0.##} сек {0}",
                Color = OxyColors.Brown,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true
            };

            ITCASeries = new LineSeries
            {
                Title = "A IT8516C+",
                TrackerFormatString = "{4:0.###} A {2:0.##} сек {0}",
                Color = OxyColors.RosyBrown,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true
            };

            ITCWSeries = new LineSeries
            {
                Title = "W IT8516C+",
                TrackerFormatString = "{4:0.###} A {2:0.##} сек {0}",
                Color = OxyColors.SandyBrown,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true
            };


            AKIPVSeries = new LineSeries
            {
                Title = "V АКИП",
                TrackerFormatString = "{4:0.###} В {2:0.##} сек {0}",
                Color = OxyColors.Violet,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true
            };

            AKIPASeries = new LineSeries
            {
                Title = "A АКИП",
                TrackerFormatString = "{4:0.###} A {2:0.##} сек {0}",
                Color = OxyColors.BlueViolet,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true
            };

            AKIPWSeries = new LineSeries
            {
                Title = "W АКИП",
                TrackerFormatString = "{4:0.###} W {2:0.##} сек {0}",
                Color = OxyColors.DarkViolet,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Cross,
                MarkerSize = 1,
                IsVisible = true
            };

            PlotModel1.Series.Add(TIBXASeries);
            PlotModel1.Series.Add(TBSCSeries);

            PlotModel1.Series.Add(V27Series);
            PlotModel1.Series.Add(I27Series);
        
            PlotModel1.Series.Add(V100Series);
            PlotModel1.Series.Add(I100Series);

            PlotModel1.Series.Add(ITCVSeries);
            PlotModel1.Series.Add(ITCASeries);
            PlotModel1.Series.Add(ITCWSeries);

            PlotModel1.Series.Add(AKIPVSeries);
            PlotModel1.Series.Add(AKIPASeries);
            PlotModel1.Series.Add(AKIPWSeries);


            PlotModel1.Legends.Add(new OxyPlot.Legends.Legend()
            {
                LegendTitle = "",
                LegendFontSize = 14
            });
            TIBXASeriesVisible = true;
            TBSCSeriesVisible = true;
            I27SeriesVisible = true;
            V27SeriesVisible = true;
            AKIPASeriesVisible = true;
            AKIPVSeriesVisible = true;
            AKIPWSeriesVisible = true;
            ITCASeriesVisible = true;
            ITCVSeriesVisible = true;
            ITCWSeriesVisible = true;
            V100SeriesVisible = true;
            I100SeriesVisible = true;




        }


    }
}
