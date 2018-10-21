using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Serenity_Space_Shell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public class Record
    {
        char axis;
        int coordinate;
        Record(byte[] rawbytes, int start = 0)
        {
            this.axis = System.BitConverter.ToChar(rawbytes, start);
            start += 1;
            this.coordinate = System.BitConverter.ToInt32(rawbytes, start);
            start += 4;
        }
    }

    private SerialPort port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);

    public class NetRow {
        int? Number;
        int? FirstColumn = null;
        int? SecondColumn = null;
        int? ThirdColumn = null;
        int? FourthColumn = null;
        int? FifthColumn = null;
        int? SixColumn = null;


        public NetRow(int number) {
            this.Number = number;
        }
        
    }
    public partial class MainWindow : Window
    {
        int nextByte = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i<=6; i ++)
                dataGrid1.Items.Add(i);

            var btn = new DataGridViewButtonColumn();

            foreach (DataGridViewRow row in dataGrid1.)
            {
                row.Cells[0].Value = i;
                i++;
            }
        }
    }
}
