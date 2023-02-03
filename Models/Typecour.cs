using System;
using System.Collections.Generic;

namespace warriorTime.Models
{
    public partial class Typecour
    {
        public Typecour()
        {
            Cours = new HashSet<Cour>();
        }

        public int IdTypeCours { get; set; }
        public string LibelleCours { get; set; } = null!;

        public virtual ICollection<Cour> Cours { get; set; }
    }
}
