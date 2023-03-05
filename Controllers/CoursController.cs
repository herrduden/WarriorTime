using Microsoft.AspNetCore.Mvc;
using warriorTime.Models;

namespace warriorTime.Controllers
{
    public class CoursController : Controller
    {
        private readonly WarriorTimeContext _context;
        public CoursController(WarriorTimeContext context) { 
        
            _context= context;
        }
        public IActionResult GetCours()
        {
            Console.WriteLine("bien");
            /*on remonte depuis la base de donnee les cours suceptible d'interesser l'etudiant */

            var allCours = (
                //from inscrit in _context.Inscrits.Where(i => !i.IdEtudiant.Equals(HttpContext.Session.GetInt32("id")))
/* A REVOIR POUR INCLURE LES COURS DE J > A 2H AVANT DEMARRAGE */
                from cours in _context.Cours.Where(c => c.DateCours > DateOnly.FromDateTime(DateTime.UtcNow.Date) && c.Statut.Equals("maintenu"))
                //on inscrit.IdCours equals cours.IdCours
                join discipline in _context.Disciplines
                on cours.IdDiscipline equals discipline.IdDiscipline
                join coach in _context.Coaches
                on cours.IdCoach equals coach.IdCoach
                join salle in _context.Salles
                on cours.IdsalleDeClasse equals salle.IdsalleDeClasse
                join typeCours in _context.Typecours
                on cours.IdTypeCours equals typeCours.IdTypeCours
                join inscrit in _context.Inscrits
                on cours.IdCours  equals inscrit.IdCours
                
                
                select new
                {
                    idCours = cours.IdCours,
                    duree = cours.Duree,
                    limiteEtudiant = cours.LimiteEtudiant,
                    pour = cours.Pour,
                    dateCours = cours.DateCours,
                    nomPrenomCoach = coach.Nom + " " + coach.Prenom,
                    libelleDiscipline= discipline.Discipline1,
                    equipement = discipline.Equipement,                  
                    nomSalle = salle.Nom,
                    adresse= salle.Adresse,
                    ville = salle.Ville,
                    maximum = salle.Capacite,
                    typeCours= typeCours.LibelleCours
                 
                }
                
                ).ToList();
            ViewBag.allCours = allCours;
            return View();
        }
/* Cette methode recoit un parametre idCours*/
        [Route("/Cours/Register/{idCours}")]
        public IActionResult Register(int idCours)
        {
            Console.WriteLine(idCours);
            var register = new Inscrit();
            register.IdCours = idCours;
            register.IdEtudiant = (int)HttpContext.Session.GetInt32("id");
            register.StudentStatus = true;
            _context.Inscrits.Add(register);
            _context.SaveChanges();
            return RedirectToAction(actionName:"GetCours",controllerName:"Cours");
        }

    }
}
