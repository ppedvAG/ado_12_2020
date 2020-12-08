using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HalloBinding
{
    public partial class Form1 : Form
    {
        List<Auto> autos = new List<Auto>();

        public Form1()
        {
            InitializeComponent();

            textBox2.DataBindings.Add("Text", textBox1, nameof(TextBox.Text));
            textBox2.DataBindings.Add(nameof(TextBox.BackColor), textBox1, nameof(TextBox.Text), true);

            label1.DataBindings.Add(nameof(label1.Text), trackBar1, nameof(trackBar1.Value));
            label1.DataBindings.Add(nameof(label1.Width), trackBar1, nameof(trackBar1.Value), true);

            autos.Add(new Auto() { Id = 1, Hersteller = "Baudi", Modell = "B35", PS = 176, Baujahr = new DateTime(2002, 2, 2) });
            autos.Add(new Auto() { Id = 2, Hersteller = "Baudi", Modell = "B17", PS = 923, Baujahr = new DateTime(2022, 12, 22) });
            autos.Add(new Auto() { Id = 3, Hersteller = "Bord", Modell = "G99", PS = 123, Baujahr = new DateTime(2012, 12, 12) });
            autos.Add(new Auto() { Id = 4, Hersteller = "Boyota", Modell = "T99", PS = 1123, Baujahr = new DateTime(2013, 3, 3) });

            
            bs.DataSource = autos;


            dataGridView1.DataSource = bs;
            listBox1.DataSource = bs;
            //listBox1.DisplayMember = "Hersteller";
            listBox1.Format += ListBox1_Format;
            comboBox1.DataSource = bs;

            var binding = new Binding(nameof(TextBox.Text), bs,"",true);
            binding.Format += Binding_Format;
            label2.DataBindings.Add(binding);

        }

        BindingSource bs = new BindingSource();
        private void Binding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value is Auto a)
                e.Value = $"{a.Hersteller} aus dem Jahr {a.Baujahr:d}";
        }

        private void ListBox1_Format(object sender, ListControlConvertEventArgs e)
        {
            if (e.Value is Auto a)
                e.Value = $"Auto: {a.Hersteller} {a.Modell}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hallo");
        }
    }

    class Auto
    {
        public int Id { get; set; }
        public string Hersteller { get; set; }
        public string Modell { get; set; }
        public int PS { get; set; }
        public DateTime Baujahr { get; set; }
    }
}
