using System;
using System.Collections.Generic;

namespace warriorTime.Models
{
    public partial class Coach
    {
        public Coach()
        {
            Cours = new HashSet<Cour>();
        }

        public int IdCoach { get; set; }
        public string Nom { get; set; } = null!;
        public string Prenom { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public string Tel { get; set; } = null!;
        public string Mdp { get; set; } = null!;

        public virtual ICollection<Cour> Cours { get; set; }
    }
}
