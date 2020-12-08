using System;

namespace HalloLinq
{
    public class Katze
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime GebDatum { get; set; }
        public double Gewicht { get; set; }
        public Katzenart Art { get; set; }
    }

}
