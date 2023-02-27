using Microsoft.AspNetCore.Mvc;
using warriorTime.Models;

namespace warriorTime.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly WarriorTimeContext _context;
        public ActivitiesController(WarriorTimeContext context) { 
            _context= context;
        
        }
        public IActionResult FuturActivities()
        {
            Console.WriteLine("futur");
            var futurActivities = (from etudiant in _context.Etudiants.Where(s => s.IdEtudiant == HttpContext.Session.GetInt32("id"))
                                   join inscrit in _context.Inscrits on etudiant.IdEtudiant equals inscrit.IdEtudiant
                                   join cours in _context.Cours
                                   on inscrit.IdCours equals cours.IdCours
                                   join discipline in _context.Disciplines
                                   on cours.IdDiscipline equals discipline.IdDiscipline
                                   join coach in _context.Coaches
                                   on cours.IdCoach equals coach.IdCoach
                                   join salle in _context.Salles
                                   on cours.IdsalleDeClasse equals salle.IdsalleDeClasse
                                   join typecours in _context.Typecours
                                   on cours.IdTypeCours equals typecours.IdTypeCours
                                   where cours.DateCours >= DateOnly.FromDateTime(DateTime.UtcNow.Date)
                                   select new
                                   {
                                       nomEtudiant = etudiant.Nom,
                                       prenomEtudiant = etudiant.Prenom,
                                       nomCoach = coach.Nom,
                                       prenomCoach = coach.Prenom,
                                       libelleDiscipline = discipline.Discipline1,
                                       limitPlace = cours.LimiteEtudiant,
                                       etatDuCours = cours.Statut,
                                       dateCours = cours.DateCours,
                                       dureeCours = cours.Duree,
                                       coursPour = cours.Pour,
                                       nomTypeCours = typecours.LibelleCours,
                                       equipement = discipline.Equipement,
                                       etatInscription = inscrit.StudentStatus,
                                       nomSalle = salle.Nom,
                                       capacite = salle.Capacite,
                                       Lieu=salle.Adresse

                                   }
                                      ).ToList();

            ViewBag.futurActivities = futurActivities;

            return View();
        }

        public IActionResult PastActivities()
        {
            Console.WriteLine("past");
            return View();
        }
    }
}
