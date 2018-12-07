using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace TPL_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource cancelToken = new CancellationTokenSource();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_StartAllThreads_Click(object sender, RoutedEventArgs e)
        {
            
            StartAllThreads();
        }

        private void StartAllThreads()
        {
            Task.Factory.StartNew(() => Method1());
            Task.Factory.StartNew(() => Method2());

            //Task.Run(() => Method1());
            //Task.Run(() => Method2());

            //ThreadPool.QueueUserWorkItem(StartMethod);
            //ThreadPool.QueueUserWorkItem(StartMethod2);

            //Method1();
            //Method2();
        }

        private void StartMethod2(object state)
        {
            Method2();
        }

        private void StartMethod(object state)
        {
            Method1();
        }

        private void ShowThreadInfo(object info,Label lbl , Thread thread)
        {
            this.Dispatcher.Invoke(() =>
            {
                lbl.Content = " ( " + info.ToString() + " ) Thread ID = " + 
                thread.ManagedThreadId + " | State = " + Thread.CurrentThread.ThreadState;
            });
        }
        private void Method2()
        {
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.CancellationToken = cancelToken.Token;

            //Console.WriteLine("\n ***** Method 2 ***** ");
            Thread t = Thread.CurrentThread;
            ShowThreadInfo("Method 2",lbl_Thread_2_Status ,t);

            //for (int i = 0; i < 15; i++)
            //{
            //    AppendToLabel(" "+i , lbl_Thread_2_Status);
            //    Thread.Sleep(300);
            //}

            try
            {
                Parallel.For(0, 40, parallelOptions, (i) => {
                    parallelOptions.CancellationToken.ThrowIfCancellationRequested();
                    AppendToLabel(" " + i, lbl_Thread_2_Status);
                    Thread.Sleep(300);
                });
                //this.Invoke((Action)delegate 
                //{
                //    this.ex
                //});
            }
            catch (OperationCanceledException ex)
            { 
                //this.Title = ex.Message;
            }



        }

        private void AppendToLabel(string v , Label lbl)
        {
            this.Dispatcher.Invoke(() =>
            {
                lbl.Content += v;
            });
        }

        private void Method1()
        {
            //Console.WriteLine("\n ***** Method 2 ***** ");
            Thread t = Thread.CurrentThread;

            ShowThreadInfo("Method 1", lbl_Thread_1_Status , t);
            //for (int i = 0; i < 15; i++)
            //{
            //    AppendToLabel(" " + i, lbl_Thread_1_Status);
            //    Thread.Sleep(300);
            //}

            Parallel.For(0, 40, (i) => 
            {
                AppendToLabel(" " + i, lbl_Thread_1_Status);
                Thread.Sleep(300);
            });
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            cancelToken.Cancel();
        }
    }
}
