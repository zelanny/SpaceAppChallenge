using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace SpaceSkin
{
    public partial class Form1 : Form
    {
        NodeState[,] nodeStates = new NodeState[6, 6];
        List<int> brokenTopCoordinate = new List<int>();
        List<int> brokenLeftCoordinate = new List<int>();

        int byteCounter = 0;
        SerialPort mySerialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            //mySerialPort.Open();


            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 6; j++)
                    nodeStates[i, j] = NodeState.Alive;

            dataGridView1.RowCount = 6;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.HeaderCell.Value = string.Format("{0}", row.Index + 1);
            }

            for (var k = 0; k < 6; k++)
            {
                var btn = new DataGridViewButtonColumn
                {
                    Name = (k+1).ToString()

                };
                btn.DefaultCellStyle.BackColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                dataGridView1.Columns.Add(btn);
            }

            dataGridView1.Columns.RemoveAt(0);
        }

        void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            byte[] indata = Encoding.ASCII.GetBytes(sp.ReadExisting());

            var recordList = new List<Record>();

            do
            {
                recordList.Add(new Record(indata, ref byteCounter));
            } while (byteCounter < indata.Length);

            CheckForBroken(recordList);

        }

        public void CheckForBroken(List<Record> recordList)
        {
            var topCoordinates = recordList.FindAll(x => x.Axis == 't');
            var leftCoordinates = recordList.FindAll(y => y.Axis == 'l');

            foreach (var topCoord in topCoordinates)
                foreach (var leftCoord in leftCoordinates)
                {
                    if (nodeStates[leftCoord.Coordinate - 1, topCoord.Coordinate - 1] != NodeState.Broken)
                    {
                        nodeStates[leftCoord.Coordinate, topCoord.Coordinate] = NodeState.Broken;
                        brokenLeftCoordinate.Add(leftCoord.Coordinate);
                        brokenTopCoordinate.Add(topCoord.Coordinate);
                        dataGridView1.Rows[leftCoord.Coordinate - 1].Cells[topCoord.Coordinate - 1].Style.BackColor = Color.Red;
                    }
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var testRecordList = new List<Record>
            {
                new Record('t', 1),
                new Record('l', 2),
                new Record('t',2),
                new Record('t',3),
                new Record('l', 3),
            };

            CheckForBroken(testRecordList);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentCell.Style.BackColor = Color.Yellow;
            var x = dataGridView1.CurrentCell.RowIndex;
            var y = dataGridView1.CurrentCell.ColumnIndex;
            nodeStates[x, y] = NodeState.Fixed;
        }
    }

    public class Record
    {
        public char Axis { get; set; }
        public int Coordinate { get; set; }

        public Record(char axis, int coordinate)
        {
            Axis = axis;
            Coordinate = coordinate;
        }

        public Record(byte[] rawbytes, ref int start)
        {
            Axis = BitConverter.ToChar(rawbytes, start);
            start += 1;
            Coordinate = BitConverter.ToInt32(rawbytes, start);
            start += 4;
        }
    }

    public enum NodeState
    {
        Alive,
        Fixed,
        Broken
    };
}
