using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using warriorTime.BluePrintForm;
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
                from cours in _context.Cours.Where(c => c.DateCours > DateOnly.FromDateTime(DateTime.UtcNow.Date) && c.Statut.Equals("maintenu")) //&& _context.Inscrits.Contains(c.IdCours)
                                                                                                                                                  //.Where(i => _context.Inscrits.Where(t => t.id) )

                    //on inscrit.IdCours equals cours.IdCours
                join discipline in _context.Disciplines
                on cours.IdDiscipline equals discipline.IdDiscipline
                join coach in _context.Coaches
                on cours.IdCoach equals coach.IdCoach
                join salle in _context.Salles
                on cours.IdsalleDeClasse equals salle.IdsalleDeClasse
                join typeCours in _context.Typecours
                on cours.IdTypeCours equals typeCours.IdTypeCours
                where !(
                    from inscrit in _context.Inscrits
                    where inscrit.IdEtudiant == HttpContext.Session.GetInt32("id")
                    select inscrit.IdCours
                    ).Contains(cours.IdCours)



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

        // ######################################COACH ###################################

        public IActionResult Consulter()
        {
            Console.WriteLine("consultation");
            
            var coursProf = (from cours in _context.Cours.Where(prof => prof.IdCoach == HttpContext.Session.GetInt32("id"))

                   //on inscrit.IdCours equals cours.IdCours
               join discipline in _context.Disciplines
               on cours.IdDiscipline equals discipline.IdDiscipline
               join coach in _context.Coaches
               on cours.IdCoach equals coach.IdCoach
               join salle in _context.Salles
               on cours.IdsalleDeClasse equals salle.IdsalleDeClasse
               join typeCours in _context.Typecours
               on cours.IdTypeCours equals typeCours.IdTypeCours



               select new
               {
                   idCours = cours.IdCours,
                   duree = cours.Duree,
                   limiteEtudiant = cours.LimiteEtudiant,
                   pour = cours.Pour,
                   dateCours = cours.DateCours,
                   nomPrenomCoach = coach.Nom + " " + coach.Prenom,
                   libelleDiscipline = discipline.Discipline1,
                   equipement = discipline.Equipement,
                   nomSalle = salle.Nom,
                   adresse = salle.Adresse,
                   ville = salle.Ville,
                   maximum = salle.Capacite,
                   typeCours = typeCours.LibelleCours

               }
                
                ).ToList();
            ViewBag.size = coursProf.Count();
            ViewBag.CoursProf = coursProf;
            return View();
        }

        public IActionResult planifier()
        {
            Console.WriteLine("planification");
            var typeCours = (from tc in _context.Typecours select new { tc.IdTypeCours, tc.LibelleCours }).ToList();
            var salle = (from s in _context.Salles select new { s.IdsalleDeClasse, s.Adresse, s.Capacite, s.Ville, s.Nom }).ToList();
            var discipline = (from d in _context.Disciplines select new {libelle=d.Discipline1,d.Equipement,d.IdDiscipline }).ToList();
            ViewBag.typeCours=typeCours;
            ViewBag.salle = salle;  
            ViewBag.discipline = discipline;
            return View();
        }

        public IActionResult CreerCours(BluePrintCreationCours data)
        {
            try
            {
                Console.WriteLine(data.Date); /*format string*/
                var insertionNewCours = new Cour();
                /* on creer une instance de notre table cours
                    data.X -> data qu'on recupere du blueprint
                    on lui dit que c'est egal au champs de notre base de donnes ->  insertionNewCours.X = 
                 */
                insertionNewCours.Pour = data.Pour;
                insertionNewCours.IdCoach = HttpContext.Session.GetInt32("id");
                insertionNewCours.DateCours = DateOnly.ParseExact(data.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                insertionNewCours.LimiteEtudiant = data.Limite;
                insertionNewCours.Duree = data.Duree;
                insertionNewCours.IdsalleDeClasse = data.IdSalle;
                insertionNewCours.IdDiscipline = data.IdDiscipline;
                insertionNewCours.IdTypeCours = data.IdTypeCours;
                insertionNewCours.Statut = "maintenu";
                /* ligne du dessous permet l'insertion de la ligne dans la Database*/
                Console.WriteLine(insertionNewCours.DateCours);
                _context.Cours.Add(insertionNewCours);
                var status = _context.SaveChanges();
                if (status > 0)
                {
                    TempData["insertionCours"] = 1;
                }
                else
                {
                    TempData["insertionCours"] = 0;
                }
            }
            catch (Exception)
            {
                TempData["insertionCours"] = 0;

            }
            

            


            return RedirectToAction(actionName:"planifier", controllerName:"Cours");
        }
        

        [Route("/Cours/PostPone/{idCours}")]     
        public IActionResult PostPone(int idCours)
        {
            var cours = _context.Cours.Where(c => c.IdCours == idCours).FirstOrDefault();
            ViewBag.DateCours = cours.DateCours;
            ViewBag.IdCours = idCours;
            return View();
        }

        public IActionResult PostPoneUpdate(BluePrintPostPone data)
        {
            TempData["modifyDate"] = 1;
            if (DateTime.ParseExact(data.Date,"yyyy-MM-dd",CultureInfo.InvariantCulture) > DateTime.UtcNow.Date)
            {
                var cours = _context.Cours.Where(c => c.IdCours == data.IdCours).FirstOrDefault();
                cours.DateCours = DateOnly.ParseExact(data.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                _context.SaveChanges();
            }
            else
            {
                TempData["modifyDate"] = 0;
            }
          
            return RedirectToAction(actionName: "consulter", controllerName: "Cours");

        }


        [Route("/Cours/Cancel/{idCours}")]
        public IActionResult Cancel(int idCours)
        {
            var cours = _context.Cours.Where(c => c.IdCours == idCours).FirstOrDefault();
            cours.Statut = "annule";
            _context.SaveChanges();

            return RedirectToAction(actionName: "consulter", controllerName: "Cours");
        }

        [Route("/Cours/delete/{idCours}")]
        public IActionResult Delete(int idCours)
        {
            TempData["coursDelete"] = 1;
            try
            {
                var cours = _context.Cours.Where(c => c.IdCours == idCours).FirstOrDefault();
                _context.Cours.Remove(cours);
                
            }
            catch (Exception ex)
            {
                var cours = _context.Cours.Where( x => x.IdCours==idCours ).FirstOrDefault();
                cours.Statut = "annule";
                
              
            }
            _context.SaveChanges();
           
            return RedirectToAction(actionName: "consulter", controllerName: "Cours");
        }

        [Route("/Cours/MoreDetails/{idCours}")]
        public IActionResult MoreDetails(int idCours)
        {
            var studentInscrit = (
                from etudiant in _context.Etudiants
                join inscrit in _context.Inscrits
                on etudiant.IdEtudiant equals inscrit.IdEtudiant
                where inscrit.IdCours == idCours
                select new
                {
                    nameStudent = etudiant.Nom,
                    fnamestudent = etudiant.Prenom,
                    mailStudent = etudiant.Email,
                    statusEtudiant = inscrit.StudentStatus,
                    telStudent = etudiant.Telephone
                }

                ).ToList();

            ViewBag.etudiantInscrit = studentInscrit;
            /* recuperer les info de l etudiant inscrit au cours*/
            return View();
        }

    }
}
