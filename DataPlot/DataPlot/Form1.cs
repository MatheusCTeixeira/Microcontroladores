using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using OxyPlot.Axes;

namespace DataPlot
{
    public partial class Form1 : Form
    {
        private PlotView temperaturaPlot;
        private PlotView erroPlot;
        private PlotView controlePlot;
        
        private PlotModel temperaturaModel;
        private PlotModel erroModel;
        private PlotModel controleModel;

        private LineSeries y; // Saída da plata
        private LineSeries u; // Entrada 
        private LineSeries uPP; // Entrada 
        private LineSeries e; // Erro = Saída da planta - entrada

        private const int TEMPERATURA_MAXIMA = 80; //Temperatura máxima
        private const int TEMPERATURA_MINIMA = 35; //Temperatura mínima

       

        void enviarTemperatura(int temperatura)
        {
            if (temperatura < TEMPERATURA_MINIMA || temperatura > TEMPERATURA_MAXIMA) //Out of range
                return;

            if (Serial.IsOpen)
            {
                string data = Convert.ToString(temperatura);
                Serial.WriteLine(data);
            }
        }

        float lerTemperatura()
        {
            try
            {
                if (Serial.IsOpen)
                {
                    string data = Serial.ReadLine();
                    float value = 0;
                    if (float.TryParse(data, out value))
                        return value;
                }
            }
            catch(TimeoutException e) { }

            return 0.0f;
        }

        public PlotView createPlot()
        {
            PlotView plot;
            plot = new OxyPlot.WindowsForms.PlotView();
            plot.Dock = System.Windows.Forms.DockStyle.Fill;
            plot.Location = new System.Drawing.Point(0, 0);
            plot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            plot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            plot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;

            return plot;
        }

        public PlotModel createPlotModel(string title, Series[] series)
        {
            PlotModel plotModel;
            plotModel = new PlotModel { Title = title };
            foreach (var serie in series)
                plotModel.Series.Add(serie);

            return plotModel;
        }

        public Form1()
        {
            InitializeComponent();

            this.temperaturaPlot = createPlot();
            this.erroPlot = createPlot();
            this.controlePlot = createPlot();            

            this.y = new LineSeries();
            this.u = new LineSeries();
            this.e = new LineSeries();
            this.uPP = new LineSeries();

            LineSeries[] arrYU = new LineSeries[2];
            arrYU[0] = y;
            arrYU[1] = u;
            temperaturaModel = createPlotModel("Temperatura (ºC)", arrYU);
            temperaturaPlot.Model = temperaturaModel;

            LineSeries[] arrE = new LineSeries[1];
            arrE[0] = e;
            erroModel = createPlotModel("Erro (ºC)", arrE);
            erroPlot.Model = erroModel;

            LineSeries[] arrU = new LineSeries[1];
            arrU[0] = uPP;
            controleModel = createPlotModel("Ação de controle (%)", arrU);
            controlePlot.Model = controleModel;


            this.panel1.Controls.Add(temperaturaPlot);
            this.panel2.Controls.Add(controlePlot);
            this.panel3.Controls.Add(erroPlot);

            lblTemperaturaMax.Text = Convert.ToString(TEMPERATURA_MAXIMA) + "ºC";            
            lblTemperaturaMin.Text = Convert.ToString(TEMPERATURA_MINIMA) + "ºC";

            trbTemperaturaControle.SetRange(TEMPERATURA_MINIMA, TEMPERATURA_MAXIMA);

            cmbPorta.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        int a = 0;
        double b = 0;
        Random r = new Random();
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            double erro = b - trbTemperaturaControle.Value;
            this.y.Points.Add(new DataPoint(a, lerTemperatura()));
            this.u.Points.Add(new DataPoint(a, trbTemperaturaControle.Value));
            this.e.Points.Add(new DataPoint(a, erro));
            this.uPP.Points.Add(new DataPoint(a, (erro / 60) * 100));

            if (this.y.Points.Count > 200)
            {
                this.y.Points.RemoveRange(0, 100);              

            }

            temperaturaPlot.Invalidate();
            erroPlot.Invalidate();
            controlePlot.Invalidate();

            a += 1;
            b = trbTemperaturaControle.Value + 0.4 * trbTemperaturaControle.Value * r.NextDouble();
            
        }

        private void cmbPorta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Serial.IsOpen)
                Serial.Close();

            Serial.PortName = (String)cmbPorta.SelectedItem;
            Serial.Open();

            timer1.Enabled = true;
           
        }

        private void cmbPorta_Click(object sender, EventArgs e)
        {
            string[] nomePortasSerial = System.IO.Ports.SerialPort.GetPortNames();

            cmbPorta.Items.Clear(); //Retira valores antigos

            foreach (var nomePorta in nomePortasSerial) // Adiciona atualizados
                cmbPorta.Items.Add(nomePorta);

        }

        private void trbTemperaturaControle_ValueChanged(object sender, EventArgs e)
        {
            enviarTemperatura(trbTemperaturaControle.Value);
        }
    }

    
}
