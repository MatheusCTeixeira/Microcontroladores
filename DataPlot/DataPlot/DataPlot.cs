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
        private LineSeries r; // Entrada 
        private LineSeries uPP; // Entrada 
        private LineSeries e; // Erro = Saída da planta - entrada

        private const int TEMPERATURA_MAXIMA = 80; //Temperatura máxima
        private const int TEMPERATURA_MINIMA = 35; //Temperatura mínima

        int a = 0;
        float temperatura = 0;
        float u_atual = 0;
        float u_anterior = 0;
        float e_atual = 0;
        float e_anterior;
        float setPoint = 0;
        const float Kp = 0.9f;
        const float Ti = 40;
        const float T = 1;

        private const string REQUEST_SIGNAL = "RQT";
        private const string ACKNOWLEDGE_SIGNAL = "ACK";

        private string serialStatus;       

        void enviarTemperatura(int temperatura)
        {
            if (temperatura < TEMPERATURA_MINIMA || temperatura > TEMPERATURA_MAXIMA) //Out of range
                return;

            serialStatus = "Enviando temperatura...";

            if (Serial.IsOpen)
            {
                string data = Convert.ToString(temperatura);
                Serial.WriteLine(data);
                serialStatus = "Temperatura enviada";
            }
            else
                serialStatus = "Não foi possível enviar a temperatura";
        }

        float lerTemperatura()
        {
            try
            {
                serialStatus = "Lendo a temperatura...";

                if (Serial.IsOpen)
                {
                    serialStatus = "Aguardado a transmissão...";
                    string data = Serial.ReadLine();
                    float value = 0;
                    if (float.TryParse(data, out value))
                    {
                        serialStatus = "Temperatura lida com sucesso";
                        return value;
                    }
                }
            }
            catch(TimeoutException e) { }

            serialStatus = "Não foi possível ler a temperatura";
            return float.NaN;
        }

        bool lerSerial()
        {
            try
            {
                serialStatus = "Lendo a porta serial...";

                if (Serial.IsOpen)
                {
                    serialStatus = "Aguardado a transmissão...";
                    string data = Serial.ReadLine();
                    char valueOf = data.ElementAt(0);
                    data = data.Substring(1);

                    if (valueOf == 't')
                    {
                        if (float.TryParse(data, out temperatura))
                        {
                            serialStatus = "Temperatura lida com sucesso";
                        }                        
                            
                    }
                    else if (valueOf == 'u')
                    {
                        if (float.TryParse(data, out u_atual))
                        {
                            serialStatus = "Ação de controle lida com sucesso";
                        }
                    }

                    return true;
                }
            }
            catch (TimeoutException e) { }

            serialStatus = "Não foi possível ler a temperatura";
            return false;
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
            this.r = new LineSeries();
            this.e = new LineSeries();
            this.uPP = new LineSeries();

            LineSeries[] arrYU = new LineSeries[2];
            arrYU[0] = y;
            arrYU[1] = r;
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
        
        private void timer1_Tick(object sender, EventArgs e)
        {            
            txtSerialStatus.Text = serialStatus;
             

            if (!lerSerial())
                return;            

            setPoint = trbTemperaturaControle.Value;

            e_anterior = e_atual;
            e_atual = setPoint - temperatura;

            u_anterior = u_atual;
            u_atual = (u_anterior + Kp * e_atual - Kp * (1 - T / Ti) * e_anterior);

            this.y.Points.Add(new DataPoint(a, temperatura));
            this.r.Points.Add(new DataPoint(a, setPoint));
            this.e.Points.Add(new DataPoint(a, e_atual));
            this.uPP.Points.Add(new DataPoint(a, u_atual));

            temperaturaPlot.Invalidate();            
            controlePlot.Invalidate();
            erroPlot.Invalidate();

            a += 1;
            
        }

        private void cmbPorta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Serial.IsOpen)
                Serial.Close();

            Serial.PortName = (String)cmbPorta.SelectedItem;
            Serial.Open();           
           
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

        private void trbTemperaturaControle_Scroll(object sender, EventArgs e)
        {

        }

        private void txtSerialStatus_TextChanged(object sender, EventArgs e)
        {

        }

        private void Serial_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show( Serial.ReadLine());
        }

        private void menuConfiguracao_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Serial_DataReceived_1(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            

        }

        private void versãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataPlot.InfForm form = new DataPlot.InfForm();
            form.Show();
        }
    }

    
}
