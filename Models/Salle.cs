using System;
using System.Collections.Generic;

namespace warriorTime.Models
{
    public partial class Salle
    {
        public Salle()
        {
            Cours = new HashSet<Cour>();
        }

        public int IdsalleDeClasse { get; set; }
        public string Nom { get; set; } = null!;
        public string Adresse { get; set; } = null!;
        public string Ville { get; set; } = null!;
        public string Capacite { get; set; } = null!;

        public virtual ICollection<Cour> Cours { get; set; }
    }
}
