using Bogus;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HalloLinq
{
    public partial class Form1 : Form
    {
        List<Katze> katzen = new List<Katze>();

        public Form1()
        {
            InitializeComponent();

            var faker = new Faker<Katze>("de_CH")
                .RuleFor(x => x.Id, f => f.UniqueIndex)
                .RuleFor(x => x.Name, f => f.Name.FirstName())
                .RuleFor(x => x.GebDatum, f => f.Date.Past())
                .RuleFor(x => x.Gewicht, f => f.Random.Double(1, 20))
                .RuleFor(x => x.Art, f => f.PickRandom<Katzenart>());

            faker.UseSeed(1234);

            katzen = faker.Generate(1000);
        }


        private void button1_Click(object sender, System.EventArgs e)
        {
            dataGridView1.DataSource = katzen;
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            var query = from k in katzen
                        where k.Art == Katzenart.Manul && k.Gewicht > 15
                        orderby k.GebDatum.Month, k.Gewicht descending
                        select k;

            dataGridView1.DataSource = query.ToList();
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            dataGridView1.DataSource = katzen.Where(k => k.Art == Katzenart.Manul && k.Gewicht > 15)
                                             .OrderBy(x => x.GebDatum.Month)
                                             .ThenByDescending(x => x.Gewicht)
                                             .ToList();
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show($"Summe: {katzen.Where(x => x.Art == Katzenart.Manul).Sum(x => x.Gewicht)}Kg"); ;
        }

        private void button5_Click(object sender, System.EventArgs e)
        {
            var result = katzen.FirstOrDefault(x => x.Art == Katzenart.Manul && x.Gewicht < 1);
            if (result == null)
                MessageBox.Show("Nix gefunden");
            else
                MessageBox.Show(result.Name);
        }
    }
}
