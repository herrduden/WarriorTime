using System;
using System.Collections.Generic;

namespace warriorTime.Models
{
    public partial class Cour
    {
        public int IdCours { get; set; }
        public int Duree { get; set; }
        public int? LimiteEtudiant { get; set; }
        public int? IdDiscipline { get; set; }
        public int? IdCoach { get; set; }
        public string Pour { get; set; } = null!;
        public string Statut { get; set; } = null!;
        public int? IdsalleDeClasse { get; set; }
        public DateOnly DateCours { get; set; }
        public int IdTypeCours { get; set; }

        public virtual Coach? IdCoachNavigation { get; set; }
        public virtual Discipline? IdDisciplineNavigation { get; set; }
        public virtual Typecour IdTypeCoursNavigation { get; set; } = null!;
        public virtual Salle? IdsalleDeClasseNavigation { get; set; }
    }
}
