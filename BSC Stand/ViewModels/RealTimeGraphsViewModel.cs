using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using BSC_Stand.Models;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;
namespace BSC_Stand.ViewModels
{
    internal class RealTimeGraphsViewModel : ViewModels.Base.ViewModelBase
    {

        public PlotModel PlotModel2 { get; set; }
        public PlotModel PlotModel3 { get; set; }

        public PlotModel PlotModel1 { get; set; }


        public int _SelectedGraphIndex;
        public int SelectedGraphIndex {
            get => _SelectedGraphIndex;
            set => Set(ref _SelectedGraphIndex, value);
        }
        #region LineSeries1
        public LineSeries V27Series { get; set; }
        public LineSeries I27Series { get; set; }
        public LineSeries V100Series { get; set; }
        public LineSeries I100Series { get; set; }
        public LineSeries TIBXASeries { get; set; }
        public LineSeries TBSCSeries { get; set; }
        public LineSeries ITCVSeries { get; set; }
        public LineSeries ITCASeries { get; set; }
        public LineSeries ITCWSeries { get; set; }
        public LineSeries AKIPVSeries { get; set; }
        public LineSeries AKIPASeries { get; set; }
        public LineSeries AKIPWSeries { get; set; }
        public LineSeries TetronVSeries { get; set; }
        public LineSeries TetronASeries { get; set; }
        public LineSeries TetronWSeries { get; set; }
        #endregion

        #region LineSeries2
        public LineSeries V27Series2 { get; set; }
        public LineSeries I27Series2 { get; set; }
        public LineSeries V100Series2 { get; set; }
        public LineSeries I100Series2 { get; set; }
        public LineSeries TIBXASeries2 { get; set; }
        public LineSeries TBSCSeries2 { get; set; }
        public LineSeries ITCVSeries2 { get; set; }
        public LineSeries ITCASeries2 { get; set; }
        public LineSeries ITCWSeries2 { get; set; }
        public LineSeries AKIPVSeries2 { get; set; }
        public LineSeries AKIPASeries2 { get; set; }
        public LineSeries AKIPWSeries2 { get; set; }
        public LineSeries TetronVSeries2 { get; set; }
        public LineSeries TetronASeries2 { get; set; }
        public LineSeries TetronWSeries2 { get; set; }
        #endregion

        #region LineSeries3
        public LineSeries V27Series3 { get; set; }
        public LineSeries I27Series3 { get; set; }
        public LineSeries V100Series3 { get; set; }
        public LineSeries I100Series3 { get; set; }
        public LineSeries TIBXASeries3 { get; set; }
        public LineSeries TBSCSeries3 { get; set; }
        public LineSeries ITCVSeries3 { get; set; }
        public LineSeries ITCASeries3 { get; set; }
        public LineSeries ITCWSeries3 { get; set; }
        public LineSeries AKIPVSeries3 { get; set; }
        public LineSeries AKIPASeries3 { get; set; }
        public LineSeries AKIPWSeries3 { get; set; }
        public LineSeries TetronVSeries3 { get; set; }
        public LineSeries TetronASeries3 { get; set; }
        public LineSeries TetronWSeries3 { get; set; }

        private LineSeries[,] series = new LineSeries[3,15];
        #endregion

        private DispatcherTimer UpdateDataTimer;







        #region GraphVisible
        #region Series1
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

        private bool _TetronVSeriesVisible;
        public bool TetronVSeriesVisible
        {
            get
            {
                return _TetronVSeriesVisible;
            }
            set
            {

                Set(ref _TetronVSeriesVisible, value);
                TetronVSeries.IsVisible = value;
                PlotModel1.InvalidatePlot(true);
            }
        }

        private bool _TetronASeriesVisible;
        public bool TetronASeriesVisible
        {
            get
            {
                return _TetronASeriesVisible;
            }
            set
            {

                Set(ref _TetronASeriesVisible, value);
                TetronASeries.IsVisible = value;
                PlotModel1.InvalidatePlot(true);
            }
        }

        private bool _TetronWSeriesVisible;
        public bool TetronWSeriesVisible
        {
            get
            {
                return _TetronWSeriesVisible;
            }
            set
            {

                Set(ref _TetronWSeriesVisible, value);
                TetronWSeries.IsVisible = value;
                PlotModel1.InvalidatePlot(true);
            }
        }
        #endregion
        #region Series2
        private bool _AKIPVSeriesVisible2;
        public bool AKIPVSeriesVisible2
        {
            get
            {
                return _AKIPVSeriesVisible2;
            }
            set
            {
                Set(ref _AKIPVSeriesVisible2, value);
                AKIPVSeries2.IsVisible = value;
                PlotModel2.InvalidatePlot(true);
            }
        }

        private bool _AKIPASeriesVisible2;
        public bool AKIPASeriesVisible2
        {
            get
            {
                return _AKIPASeriesVisible2;
            }
            set
            {
                Set(ref _AKIPASeriesVisible2, value);
                AKIPASeries2.IsVisible = value;
                PlotModel2.InvalidatePlot(true);
            }
        }

        private bool _AKIPWSeriesVisible2;
        public bool AKIPWSeriesVisible2
        {
            get
            {
                return _AKIPWSeriesVisible2;
            }
            set
            {
                Set(ref _AKIPWSeriesVisible2, value);
                AKIPWSeries2.IsVisible = value;
                PlotModel2.InvalidatePlot(true);
            }
        }

        private bool _ITCVSeriesVisible2;
        public bool ITCVSeriesVisible2
        {
            get
            {
                return _ITCVSeriesVisible2;
            }
            set
            {
                Set(ref _ITCVSeriesVisible2, value);
                ITCVSeries2.IsVisible = value;
                PlotModel2.InvalidatePlot(true);
            }
        }

        private bool _ITCASeriesVisible2;
        public bool ITCASeriesVisible2
        {
            get
            {
                return _ITCASeriesVisible2;
            }
            set
            {
                Set(ref _ITCASeriesVisible2, value);
                ITCASeries2.IsVisible = value;
                PlotModel2.InvalidatePlot(true);
            }
        }
        private bool _ITCWSeriesVisible2;
        public bool ITCWSeriesVisible2
        {
            get
            {
                return _ITCWSeriesVisible2;
            }
            set
            {
                Set(ref _ITCWSeriesVisible2, value);
                ITCWSeries2.IsVisible = value;
                PlotModel2.InvalidatePlot(true);
            }
        }


        private bool _V100SeriesVisible2;
        public bool V100SeriesVisible2
        {
            get
            {
                return _V100SeriesVisible2;
            }
            set
            {
                Set(ref _V100SeriesVisible2, value);
                V100Series2.IsVisible = value;
                PlotModel2.InvalidatePlot(true);
            }
        }

        private bool _I100SeriesVisible2;
        public bool I100SeriesVisible2
        {
            get
            {
                return _I100SeriesVisible2;
            }
            set
            {
                Set(ref _I100SeriesVisible2, value);
                I100Series2.IsVisible = value;
                PlotModel2.InvalidatePlot(true);
            }
        }

        private bool _I27SeriesVisible2;
        public bool I27SeriesVisible2
        {
            get
            {
                return _I27SeriesVisible2;
            }
            set
            {
                Set(ref _I27SeriesVisible2, value);
                I27Series2.IsVisible = value;
                PlotModel2.InvalidatePlot(true);
            }
        }


        private bool _V27SeriesVisible2;
        public bool V27SeriesVisible2
        {
            get
            {
                return _V27SeriesVisible2;
            }
            set
            {
                Set(ref _V27SeriesVisible2, value);
                V27Series2.IsVisible = value;
                PlotModel2.InvalidatePlot(true);
            }
        }


        private bool _TIBXASeriesVisible2;
        public bool TIBXASeriesVisible2
        {
            get
            {
                return _TIBXASeriesVisible2;
            }
            set
            {
                Set(ref _TIBXASeriesVisible2, value);
                TIBXASeries2.IsVisible = value;
                PlotModel2.InvalidatePlot(true);
            }
        }

        private bool _TBSCSeriesVisible2;
        public bool TBSCSeriesVisible2
        {
            get
            {
                return _TBSCSeriesVisible2;
            }
            set
            {

                Set(ref _TBSCSeriesVisible2, value);
                TBSCSeries2.IsVisible = value;
                PlotModel2.InvalidatePlot(true);
            }
        }

        private bool _TetronVSeriesVisible2;
        public bool TetronVSeriesVisible2
        {
            get
            {
                return _TetronVSeriesVisible2;
            }
            set
            {

                Set(ref _TetronVSeriesVisible2, value);
                TetronVSeries2.IsVisible = value;
                PlotModel2.InvalidatePlot(true);
            }
        }

        private bool _TetronASeriesVisible2;
        public bool TetronASeriesVisible2
        {
            get
            {
                return _TetronASeriesVisible2;
            }
            set
            {

                Set(ref _TetronASeriesVisible2, value);
                TetronASeries2.IsVisible = value;
                PlotModel2.InvalidatePlot(true);
            }
        }

        private bool _TetronWSeriesVisible2;
        public bool TetronWSeriesVisible2
        {
            get
            {
                return _TetronWSeriesVisible2;
            }
            set
            {

                Set(ref _TetronWSeriesVisible2, value);
                TetronWSeries2.IsVisible = value;
                PlotModel2.InvalidatePlot(true);
            }
        }
        #endregion
        #region Series3
        private bool _AKIPVSeriesVisible3;
        public bool AKIPVSeriesVisible3
        {
            get
            {
                return _AKIPVSeriesVisible3;
            }
            set
            {
                Set(ref _AKIPVSeriesVisible3, value);
                AKIPVSeries3.IsVisible = value;
                PlotModel3.InvalidatePlot(true);
            }
        }

        private bool _AKIPASeriesVisible3;
        public bool AKIPASeriesVisible3
        {
            get
            {
                return _AKIPASeriesVisible3;
            }
            set
            {
                Set(ref _AKIPASeriesVisible3, value);
                AKIPASeries3.IsVisible = value;
                PlotModel3.InvalidatePlot(true);
            }
        }

        private bool _AKIPWSeriesVisible3;
        public bool AKIPWSeriesVisible3
        {
            get
            {
                return _AKIPWSeriesVisible3;
            }
            set
            {
                Set(ref _AKIPWSeriesVisible3, value);
                AKIPWSeries3.IsVisible = value;
                PlotModel3.InvalidatePlot(true);
            }
        }

        private bool _ITCVSeriesVisible3;
        public bool ITCVSeriesVisible3
        {
            get
            {
                return _ITCVSeriesVisible3;
            }
            set
            {
                Set(ref _ITCVSeriesVisible3, value);
                ITCVSeries3.IsVisible = value;
                PlotModel3.InvalidatePlot(true);
            }
        }

        private bool _ITCASeriesVisible3;
        public bool ITCASeriesVisible3
        {
            get
            {
                return _ITCASeriesVisible3;
            }
            set
            {
                Set(ref _ITCASeriesVisible3, value);
                ITCASeries3.IsVisible = value;
                PlotModel3.InvalidatePlot(true);
            }
        }
        private bool _ITCWSeriesVisible3;
        public bool ITCWSeriesVisible3
        {
            get
            {
                return _ITCWSeriesVisible3;
            }
            set
            {
                Set(ref _ITCWSeriesVisible3, value);
                ITCWSeries3.IsVisible = value;
                PlotModel3.InvalidatePlot(true);
            }
        }


        private bool _V100SeriesVisible3;
        public bool V100SeriesVisible3
        {
            get
            {
                return _V100SeriesVisible3;
            }
            set
            {
                Set(ref _V100SeriesVisible3, value);
                V100Series3.IsVisible = value;
                PlotModel3.InvalidatePlot(true);
            }
        }

        private bool _I100SeriesVisible3;
        public bool I100SeriesVisible3
        {
            get
            {
                return _I100SeriesVisible3;
            }
            set
            {
                Set(ref _I100SeriesVisible3, value);
                I100Series3.IsVisible = value;
                PlotModel3.InvalidatePlot(true);
            }
        }

        private bool _I27SeriesVisible3;
        public bool I27SeriesVisible3
        {
            get
            {
                return _I27SeriesVisible3;
            }
            set
            {
                Set(ref _I27SeriesVisible3, value);
                I27Series3.IsVisible = value;
                PlotModel3.InvalidatePlot(true);
            }
        }


        private bool _V27SeriesVisible3;
        public bool V27SeriesVisible3
        {
            get
            {
                return _V27SeriesVisible3;
            }
            set
            {
                Set(ref _V27SeriesVisible3, value);
                V27Series3.IsVisible = value;
                PlotModel3.InvalidatePlot(true);
            }
        }


        private bool _TIBXASeriesVisible3;
        public bool TIBXASeriesVisible3
        {
            get
            {
                return _TIBXASeriesVisible3;
            }
            set
            {
                Set(ref _TIBXASeriesVisible3, value);
                TIBXASeries3.IsVisible = value;
                PlotModel3.InvalidatePlot(true);
            }
        }

        private bool _TBSCSeriesVisible3;
        private readonly StatusBarViewModel _statusBarViewModel;

        public bool TBSCSeriesVisible3
        {
            get
            {
                return _TBSCSeriesVisible3;
            }
            set
            {

                Set(ref _TBSCSeriesVisible3, value);
                TBSCSeries3.IsVisible = value;
                PlotModel3.InvalidatePlot(true);
            }
        }

        private bool _TetronVSeriesVisible3;
        public bool TetronVSeriesVisible3
        {
            get
            {
                return _TetronVSeriesVisible3;
            }
            set
            {

                Set(ref _TetronVSeriesVisible3, value);
                TetronVSeries3.IsVisible = value;
                PlotModel3.InvalidatePlot(true);
            }
        }

        private bool _TetronASeriesVisible3;
        public bool TetronASeriesVisible3
        {
            get
            {
                return _TetronASeriesVisible3;
            }
            set
            {

                Set(ref _TetronASeriesVisible3, value);
                TetronASeries3.IsVisible = value;
                PlotModel3.InvalidatePlot(true);
            }
        }

        private bool _TetronWSeriesVisible3;
        public bool TetronWSeriesVisible3
        {
            get
            {
                return _TetronWSeriesVisible3;
            }
            set
            {

                Set(ref _TetronWSeriesVisible3, value);
                TetronWSeries3.IsVisible = value;
                PlotModel3.InvalidatePlot(true);
            }
        }
        #endregion




        #endregion
        public RealTimeGraphsViewModel()
        {

            UpdateDataTimer = new DispatcherTimer();
            UpdateDataTimer.Tick += UpdateDataTimer_Tick;
            UpdateDataTimer.Interval = TimeSpan.FromMilliseconds(100);
            UpdateDataTimer.Start();
            InitGraphSeries();
        }

        private  async void UpdateDataTimer_Tick(object? sender, EventArgs e)
        {
          PlotModel1.InvalidatePlot(true);
            PlotModel2.InvalidatePlot(true);
            PlotModel3.InvalidatePlot(true);
        }

        public async void  UpdateGraphsSeries(ReadingParams readingParams)
        {
            //Обновление серий
            await Task.Run(() =>
            {
                LineSeries[,] series = new LineSeries[3, 15]
                {
                    { ITCVSeries, ITCASeries, ITCWSeries, AKIPASeries, AKIPVSeries, AKIPWSeries, V27Series, I27Series, V100Series, I100Series, TIBXASeries, TBSCSeries, TetronVSeries, TetronASeries, TetronWSeries },
                    { ITCVSeries2, ITCASeries2, ITCWSeries2, AKIPASeries2, AKIPVSeries2, AKIPWSeries2, V27Series2, I27Series2, V100Series2, I100Series2, TIBXASeries2, TBSCSeries2, TetronVSeries2, TetronASeries2, TetronWSeries2 },
                    { ITCVSeries3, ITCASeries3, ITCWSeries3, AKIPASeries3, AKIPVSeries3, AKIPWSeries3, V27Series3, I27Series3, V100Series3, I100Series3, TIBXASeries3, TBSCSeries3, TetronVSeries3, TetronASeries3, TetronWSeries3 }
                };


                float[] parameters = { readingParams.ITCVValue, readingParams.ITCAValue, readingParams.ITCWValue,
                    readingParams.AKIPAValue, readingParams.AKIPVValue, readingParams.AKIPWValue,
                    readingParams.V27Value, readingParams.I27Value,
                    readingParams.V100Value, readingParams.I100Value,
                    readingParams.IBXATemperature, readingParams.BSCTemperature,
                    readingParams.TetronVValue, readingParams.TetronAValue, readingParams.TetronWValue };

                Parallel.For(0, 3, i =>
                {
                   int j = 0;
                   foreach (var param in parameters)
                   {

                       series[i, j].Points.Add(new DataPoint(readingParams.ExpTime, parameters[j]));

                       j++;
                   }
                });


            });
                
                /*
                ITCVSeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.ITCVValue));
                ITCASeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.ITCAValue));
                ITCWSeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.ITCWValue));

                AKIPASeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.AKIPAValue));
                AKIPVSeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.AKIPVValue));
                AKIPWSeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.AKIPWValue));

                V27Series.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.V27Value));
                I27Series.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.I27Value));

                V100Series.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.V100Value));
                I100Series.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.I100Value));

                TIBXASeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.IBXATemperature));
                TBSCSeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.BSCTemperature));

                ITCVSeries2.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.ITCVValue));
                ITCASeries2.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.ITCAValue));
                ITCWSeries2.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.ITCWValue));

                AKIPASeries2.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.AKIPAValue));
                AKIPVSeries2.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.AKIPVValue));
                AKIPWSeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.AKIPWValue));

                V27Series2.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.V27Value));
                I27Series2.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.I27Value));

                V100Series2.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.V100Value));
                I100Series2.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.I100Value));

                TIBXASeries2.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.IBXATemperature));
                TBSCSeries2.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.BSCTemperature));


                ITCVSeries3.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.ITCVValue));
                ITCASeries3.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.ITCAValue));
                ITCWSeries3.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.ITCWValue));

                AKIPASeries3.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.AKIPAValue));
                AKIPVSeries3.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.AKIPVValue));
                AKIPWSeries.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.AKIPWValue));

                V27Series3.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.V27Value));
                I27Series3.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.I27Value));

                V100Series3.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.V100Value));
                I100Series3.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.I100Value));

                TIBXASeries3.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.IBXATemperature));
                TBSCSeries3.Points.Add(new DataPoint(readingParams.ExpTime, readingParams.BSCTemperature));
                */
            //}
            //





            

        }
        private void InitGraphSeries()
        {
            PlotModel1 = new PlotModel();

            PlotModel1.Axes.Add(new OxyPlot.Axes.LinearAxis());

            PlotModel1.Series.Add(new LineSeries()
            {

            });


            PlotModel2 = new PlotModel();
            PlotModel2.Axes.Add(new OxyPlot.Axes.LinearAxis());
            PlotModel2.Series.Add(new LineSeries()
            {

            });

            PlotModel3 = new PlotModel();
            PlotModel3.Axes.Add(new OxyPlot.Axes.LinearAxis());
            PlotModel3.Series.Add(new LineSeries()
            {

            });

            #region InitSeries
            {

                V27Series = new LineSeries
                {
                    Title = "V 27",
                    TrackerFormatString = "{0}\n{4:0.###} В\n{2:0.##} сек",
                    Color = OxyColors.Blue,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };



                I27Series = new LineSeries
                {
                    Title = "I 27",
                    TrackerFormatString = "{0}\n{4:0.###} A\n{2:0.##} сек",
                    Color = OxyColors.BlueViolet,
                    MarkerFill = OxyColors.DarkBlue,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true

                };


                V100Series = new LineSeries
                {
                    Title = "V 100",
                    TrackerFormatString = "{0}\n{4:0} В\n{2:0} сек",
                    Color = OxyColors.DarkOrange,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };



                I100Series = new LineSeries
                {
                    Title = "I 100",
                    TrackerFormatString = "{0}\n{4:0} A\n{2:0} сек",
                    Color = OxyColors.OrangeRed,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };


                TIBXASeries = new LineSeries
                {
                    Title = "T°C  ИБХА",
                    TrackerFormatString = "{0}\n{4:0} T°C\n{2:0} сек",
                    Color = OxyColors.Green,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };

                TBSCSeries = new LineSeries
                {
                    Title = "T°C  ЭОБСК",
                    TrackerFormatString = "{0}\n{4:0} T°C\n{2:0} сек",
                    Color = OxyColors.ForestGreen,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };



                ITCVSeries = new LineSeries
                {
                    Title = "V IT8516C+",
                    TrackerFormatString = "{0}\n{4:0.###} В\n{2:0.##} сек",
                    Color = OxyColors.Brown,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                ITCASeries = new LineSeries
                {
                    Title = "A IT8516C+",
                    TrackerFormatString = "{0}\n{4:0.###} A\n{2:0.##} сек",
                    Color = OxyColors.RosyBrown,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                ITCWSeries = new LineSeries
                {
                    Title = "W IT8516C+",
                    TrackerFormatString = "{0}\n{4:0.###} A\n{2:0.##} сек",
                    Color = OxyColors.SandyBrown,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };


                AKIPVSeries = new LineSeries
                {
                    Title = "V АКИП",
                    TrackerFormatString = "{0}\n{4:0.###} В\n{2:0.##} сек",
                    Color = OxyColors.Violet,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                AKIPASeries = new LineSeries
                {
                    Title = "A АКИП",
                    TrackerFormatString = "{0}\n{4:0.###} A {2:0.##} сек",
                    Color = OxyColors.BlueViolet,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                AKIPWSeries = new LineSeries
                {
                    Title = "W АКИП",
                    TrackerFormatString = "{0}\n{4:0.###} W\n{2:0.##} сек",
                    Color = OxyColors.DarkViolet,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                TetronVSeries = new LineSeries
                {
                    Title = "V Тетрон",
                    TrackerFormatString = "{0}\n{4:0.###} В\n{2:0.##} сек",
                    Color = OxyColors.LightGray,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                TetronASeries = new LineSeries
                {
                    Title = "A Тетрон",
                    TrackerFormatString = "{0}\n{4:0.###} A\n{2:0.##} сек",
                    Color = OxyColors.Gray,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                TetronWSeries = new LineSeries
                {
                    Title = "W Тетрон",
                    TrackerFormatString = "{0}\n{4:0.###} W\n{2:0.##} сек",
                    Color = OxyColors.DarkGray,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };
            }
            ///Series 2
            {

                V27Series2 = new LineSeries
                {
                    Title = "V 27",
                    TrackerFormatString = "{4:0.###} В {2:0.##} сек",
                    Color = OxyColors.Blue,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };



                I27Series2 = new LineSeries
                {
                    Title = "I 27",
                    TrackerFormatString = "{4:0.###} A {2:0.##} сек",
                    Color = OxyColors.BlueViolet,
                    MarkerFill = OxyColors.DarkBlue,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true

                };


                V100Series2 = new LineSeries
                {
                    Title = "V 100",
                    TrackerFormatString = "{4:0} В {2:0} сек",
                    Color = OxyColors.DarkOrange,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };



                I100Series2 = new LineSeries
                {
                    Title = "I 100",
                    TrackerFormatString = "{4:0} A {2:0} сек",
                    Color = OxyColors.OrangeRed,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };


                TIBXASeries2 = new LineSeries
                {
                    Title = "T°C  ИБХА",
                    TrackerFormatString = "{4:0} T°C  {2:0} сек {0}",
                    Color = OxyColors.Green,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };

                TBSCSeries2 = new LineSeries
                {
                    Title = "T°C  ЭОБСК",
                    TrackerFormatString = "{4:0} T°C  {2:0} сек {0}",
                    Color = OxyColors.ForestGreen,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };



                ITCVSeries2 = new LineSeries
                {
                    Title = "V IT8516C+",
                    TrackerFormatString = "{4:0.###} В {2:0.##} сек {0}",
                    Color = OxyColors.Brown,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                ITCASeries2 = new LineSeries
                {
                    Title = "A IT8516C+",
                    TrackerFormatString = "{4:0.###} A {2:0.##} сек {0}",
                    Color = OxyColors.RosyBrown,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                ITCWSeries2 = new LineSeries
                {
                    Title = "W IT8516C+",
                    TrackerFormatString = "{4:0.###} A {2:0.##} сек {0}",
                    Color = OxyColors.SandyBrown,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };


                AKIPVSeries2 = new LineSeries
                {
                    Title = "V АКИП",
                    TrackerFormatString = "{4:0.###} В {2:0.##} сек {0}",
                    Color = OxyColors.Violet,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                AKIPASeries2 = new LineSeries
                {
                    Title = "A АКИП",
                    TrackerFormatString = "{4:0.###} A {2:0.##} сек {0}",
                    Color = OxyColors.BlueViolet,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                AKIPWSeries2 = new LineSeries
                {
                    Title = "W АКИП",
                    TrackerFormatString = "{4:0.###} W {2:0.##} сек {0}",
                    Color = OxyColors.DarkViolet,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                TetronVSeries2 = new LineSeries
                {
                    Title = "V Тетрон",
                    TrackerFormatString = "{4:0.###} В {2:0.##} сек {0}",
                    Color = OxyColors.LightGray,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                TetronASeries2 = new LineSeries
                {
                    Title = "A Тетрон",
                    TrackerFormatString = "{4:0.###} A {2:0.##} сек {0}",
                    Color = OxyColors.Gray,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                TetronWSeries2 = new LineSeries
                {
                    Title = "W Тетрон",
                    TrackerFormatString = "{4:0.###} W {2:0.##} сек {0}",
                    Color = OxyColors.DarkGray,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };
            }
            //Series3
            {
                V27Series3 = new LineSeries
                {
                    Title = "V 27",
                    TrackerFormatString = "{4:0.###} В {2:0.##} сек",
                    Color = OxyColors.Blue,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };



                I27Series3 = new LineSeries
                {
                    Title = "I 27",
                    TrackerFormatString = "{4:0.###} A {2:0.##} сек",
                    Color = OxyColors.BlueViolet,
                    MarkerFill = OxyColors.DarkBlue,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true

                };


                V100Series3 = new LineSeries
                {
                    Title = "V 100",
                    TrackerFormatString = "{4:0} В {2:0} сек",
                    Color = OxyColors.DarkOrange,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };



                I100Series3 = new LineSeries
                {
                    Title = "I 100",
                    TrackerFormatString = "{4:0} A {2:0} сек",
                    Color = OxyColors.OrangeRed,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };


                TIBXASeries3 = new LineSeries
                {
                    Title = "T°C  ИБХА",
                    TrackerFormatString = "{4:0} T°C  {2:0} сек {0}",
                    Color = OxyColors.Green,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };

                TBSCSeries3 = new LineSeries
                {
                    Title = "T°C  ЭОБСК",
                    TrackerFormatString = "{4:0} T°C  {2:0} сек {0}",
                    Color = OxyColors.ForestGreen,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true,
                };



                ITCVSeries3 = new LineSeries
                {
                    Title = "V IT8516C+",
                    TrackerFormatString = "{4:0.###} В {2:0.##} сек {0}",
                    Color = OxyColors.Brown,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                ITCASeries3 = new LineSeries
                {
                    Title = "A IT8516C+",
                    TrackerFormatString = "{4:0.###} A {2:0.##} сек {0}",
                    Color = OxyColors.RosyBrown,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                ITCWSeries3 = new LineSeries
                {
                    Title = "W IT8516C+",
                    TrackerFormatString = "{4:0.###} A {2:0.##} сек {0}",
                    Color = OxyColors.SandyBrown,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };


                AKIPVSeries3 = new LineSeries
                {
                    Title = "V АКИП",
                    TrackerFormatString = "{4:0.###} В {2:0.##} сек {0}",
                    Color = OxyColors.Violet,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                AKIPASeries3 = new LineSeries
                {
                    Title = "A АКИП",
                    TrackerFormatString = "{4:0.###} A {2:0.##} сек {0}",
                    Color = OxyColors.BlueViolet,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                AKIPWSeries3 = new LineSeries
                {
                    Title = "W АКИП",
                    TrackerFormatString = "{4:0.###} W {2:0.##} сек {0}",
                    Color = OxyColors.DarkViolet,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                TetronVSeries3 = new LineSeries
                {
                    Title = "V Тетрон",
                    TrackerFormatString = "{4:0.###} В {2:0.##} сек {0}",
                    Color = OxyColors.LightGray,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                TetronASeries3 = new LineSeries
                {
                    Title = "A Тетрон",
                    TrackerFormatString = "{4:0.###} A {2:0.##} сек {0}",
                    Color = OxyColors.Gray,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };

                TetronWSeries3 = new LineSeries
                {
                    Title = "W Тетрон",
                    TrackerFormatString = "{4:0.###} W {2:0.##} сек {0}",
                    Color = OxyColors.DarkGray,
                    MarkerFill = OxyColors.Red,
                    MarkerType = MarkerType.Cross,
                    MarkerSize = 1,
                    IsVisible = true
                };
            }

            #endregion

            #region AddSerieToPlot
            //Add Series to Plot Models (Порядок в легенде меняется тут)
            {
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

                PlotModel1.Series.Add(TetronVSeries);
                PlotModel1.Series.Add(TetronASeries);
                PlotModel1.Series.Add(TetronWSeries);
            }
            {
                PlotModel2.Series.Add(TIBXASeries2);
                PlotModel2.Series.Add(TBSCSeries2);

                PlotModel2.Series.Add(V27Series2);
                PlotModel2.Series.Add(I27Series2);

                PlotModel2.Series.Add(V100Series2);
                PlotModel2.Series.Add(I100Series2);

                PlotModel2.Series.Add(ITCVSeries2);
                PlotModel2.Series.Add(ITCASeries2);
                PlotModel2.Series.Add(ITCWSeries2);

                PlotModel2.Series.Add(AKIPVSeries2);
                PlotModel2.Series.Add(AKIPASeries2);
                PlotModel2.Series.Add(AKIPWSeries2);

                PlotModel2.Series.Add(TetronVSeries2);
                PlotModel2.Series.Add(TetronASeries2);
                PlotModel2.Series.Add(TetronWSeries2);
            }

            {
                PlotModel3.Series.Add(TIBXASeries3);
                PlotModel3.Series.Add(TBSCSeries3);

                PlotModel3.Series.Add(V27Series3);
                PlotModel3.Series.Add(I27Series3);

                PlotModel3.Series.Add(V100Series3);
                PlotModel3.Series.Add(I100Series3);

                PlotModel3.Series.Add(ITCVSeries3);
                PlotModel3.Series.Add(ITCASeries3);
                PlotModel3.Series.Add(ITCWSeries3);

                PlotModel3.Series.Add(AKIPVSeries3);
                PlotModel3.Series.Add(AKIPASeries3);
                PlotModel3.Series.Add(AKIPWSeries3);

                PlotModel3.Series.Add(TetronVSeries3);
                PlotModel3.Series.Add(TetronASeries3);
                PlotModel3.Series.Add(TetronWSeries3);
            }




            #endregion






            PlotModel1.Legends.Add(new OxyPlot.Legends.Legend()
            {
                LegendTitle = "",
                LegendFontSize = 14
            });


            PlotModel2.Legends.Add(new OxyPlot.Legends.Legend()
            {
                LegendTitle = "",
                LegendFontSize = 14
            });

            PlotModel3.Legends.Add(new OxyPlot.Legends.Legend()
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
            TetronVSeriesVisible = true;
            TetronASeriesVisible = true;
            TetronWSeriesVisible = true;


            TIBXASeriesVisible2 = true;
            TBSCSeriesVisible2 = true;
            I27SeriesVisible2 = true;
            V27SeriesVisible2 = true;
            AKIPASeriesVisible2 = true;
            AKIPVSeriesVisible2 = true;
            AKIPWSeriesVisible2 = true;
            ITCASeriesVisible2 = true;
            ITCVSeriesVisible2 = true;
            ITCWSeriesVisible2 = true;
            V100SeriesVisible2 = true;
            I100SeriesVisible2 = true;
            TetronVSeriesVisible2 = true;
            TetronASeriesVisible2 = true;
            TetronWSeriesVisible2 = true;

            TIBXASeriesVisible3 = true;
            TBSCSeriesVisible3 = true;
            I27SeriesVisible3 = true;
            V27SeriesVisible3 = true;
            AKIPASeriesVisible3 = true;
            AKIPVSeriesVisible3 = true;
            AKIPWSeriesVisible3 = true;
            ITCASeriesVisible3 = true;
            ITCVSeriesVisible3 = true;
            ITCWSeriesVisible3 = true;
            V100SeriesVisible3 = true;
            I100SeriesVisible3 = true;
            TetronVSeriesVisible3 = true;
            TetronASeriesVisible3 = true;
            TetronWSeriesVisible3 = true;
          

            }



        public void ClearAllPoints()
        {
            V27Series.Points.Clear();
            I27Series.Points.Clear();
            V100Series.Points.Clear();
            I100Series.Points.Clear();
            TIBXASeries.Points.Clear();
            TBSCSeries.Points.Clear(); 
            ITCVSeries.Points.Clear();
            ITCASeries.Points.Clear();
            ITCWSeries.Points.Clear();
            AKIPVSeries.Points.Clear();
            AKIPASeries.Points.Clear();
            AKIPWSeries.Points.Clear();
            TetronVSeries.Points.Clear();
            TetronASeries.Points.Clear();
            TetronWSeries.Points.Clear();

            V27Series2.Points.Clear();
            I27Series2.Points.Clear();
            V100Series2.Points.Clear();
            I100Series2.Points.Clear();
            TIBXASeries2.Points.Clear();
            TBSCSeries2.Points.Clear();
            ITCVSeries2.Points.Clear();
            ITCASeries2.Points.Clear();
            ITCWSeries2.Points.Clear();
            AKIPVSeries2.Points.Clear();
            AKIPASeries2.Points.Clear();
            AKIPWSeries2.Points.Clear();
            TetronVSeries2.Points.Clear();
            TetronASeries2.Points.Clear();
            TetronWSeries2.Points.Clear();

            V27Series3.Points.Clear();
            I27Series3.Points.Clear();
            V100Series3.Points.Clear();
            I100Series3.Points.Clear();
            TIBXASeries3.Points.Clear();
            TBSCSeries3.Points.Clear();
            ITCVSeries3.Points.Clear();
            ITCASeries3.Points.Clear();
            ITCWSeries3.Points.Clear();
            AKIPVSeries3.Points.Clear();
            AKIPASeries3.Points.Clear();
            AKIPWSeries3.Points.Clear();
            TetronVSeries3.Points.Clear();
            TetronASeries3.Points.Clear();
            TetronWSeries3.Points.Clear();

           



        }

        public void ResetAllAxes()
        {
            PlotModel1.ResetAllAxes();
            PlotModel2.ResetAllAxes();
            PlotModel3.ResetAllAxes();
        }

    }
}
