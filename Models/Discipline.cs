using System;
using System.Collections.Generic;

namespace warriorTime.Models
{
    public partial class Discipline
    {
        public Discipline()
        {
            Cours = new HashSet<Cour>();
        }

        public int IdDiscipline { get; set; }
        public string Discipline1 { get; set; } = null!;
        public int? Equipement { get; set; }

        public virtual ICollection<Cour> Cours { get; set; }
    }
}
