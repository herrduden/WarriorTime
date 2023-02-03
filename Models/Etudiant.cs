using System;
using System.Collections.Generic;

namespace warriorTime.Models
{
    public partial class Etudiant
    {
        public int IdEtudiant { get; set; }
        public string Nom { get; set; } = null!;
        public string Prenom { get; set; } = null!;
        public string Telephone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Mdp { get; set; } = null!;
    }
}
