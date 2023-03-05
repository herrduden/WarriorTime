using Microsoft.AspNetCore.Mvc;
using warriorTime.BluePrintForm;
using warriorTime.Models;

namespace warriorTime.Controllers
{
    public class InternController : Controller
        
    {
        private readonly WarriorTimeContext _context;
        public InternController(WarriorTimeContext context)
        {
            _context = context;
        }
        public IActionResult DashBoard()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("email")))
            {
                /*on recupere les donnees des sceances de sports passees avant de return le view*/
                    /* on simule une requete based on our database*/

                /* a quoi correspond le s=> s.idEtudiant ? et le resultat de la requete va t elle nous donner 1 ligne?
                 
                 lambda expression*/
                var previousActivities = (
                        from etudiant in _context.Etudiants.Where(s => s.IdEtudiant == HttpContext.Session.GetInt32("id"))
                        join inscrit in _context.Inscrits
                        on etudiant.IdEtudiant equals inscrit.IdEtudiant
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
                        where cours.DateCours < DateOnly.FromDateTime(DateTime.UtcNow.Date)
                        select new { 
                            nomEtudiant=etudiant.Nom,
                            prenomEtudiant = etudiant.Prenom,
                            nomCoach= coach.Nom,
                            prenomCoach=coach.Prenom,
/*discipline 1 faire reference a la colonne discipline*/
                            libelleDiscipline= discipline.Discipline1,
                            limitPlace=cours.LimiteEtudiant,
                            etatDuCours=cours.Statut,
                            dateCours = cours.DateCours,
                            dureeCours=cours.Duree,
                            coursPour=cours.Pour,
                            nomTypeCours=typecours.LibelleCours,
                            equipement=discipline.Equipement,
                            etatInscription=inscrit.StudentStatus,
                            nomSalle=salle.Nom,
                            capacite=salle.Capacite
                        }
                        

                    ).ToList();
                /* le toList sert a transformer le resultat de la requete en tableau*/
                ViewBag.previousActivities = previousActivities;

                /*view bag va me permettre de transporter mes donnees dans la vue`*/



                var futurActivities = (from etudiant in _context.Etudiants.Where(s=>s.IdEtudiant==HttpContext.Session.GetInt32("id"))
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
                                          capacite = salle.Capacite
                                      }
                                      ).ToList();

                ViewBag.futurActivities = futurActivities;

                return View();
/* Il faut que je m'identifie pour avoir acce a la page intern*/

            }

            TempData["PermissionDenied"] = 1;
            return RedirectToAction(actionName:"LoginPage",controllerName:"Login" );
        }


        public IActionResult InfoPerso()
        {
            var student = _context.Etudiants.Where(self => self.IdEtudiant == HttpContext.Session.GetInt32("id")).FirstOrDefault();
            ViewBag.recup = student;
            return View ();
        }

        public IActionResult UpdatePassword(BluePrintUserInfo data)
        {
            Console.WriteLine(data.OldPassword);
            Console.WriteLine(data.NewPassword);
            Console.WriteLine(data.ConfirmNewPassword);
            if (HttpContext.Session.GetString("pwd").Equals(data.NewPassword))
            {
                Console.WriteLine("le meme");
                TempData["OldNewEquals"] = 1;
                return RedirectToAction(actionName: "InfoPerso", controllerName: "Intern");


            }

            if (HttpContext.Session.GetString("pwd").Equals(data.OldPassword))
            {
                Console.WriteLine("old password match");
                if (data.NewPassword.Equals(data.ConfirmNewPassword))
                {
                    TempData["verifOk"] = 1;
                    Console.WriteLine("les mdp match !");
                    var verif = _context.Etudiants.Where(v => v.IdEtudiant == HttpContext.Session.GetInt32("id")).FirstOrDefault();

                    verif.Mdp = data.NewPassword;
                    _context.SaveChanges();
                    HttpContext.Session.SetString("pwd",data.NewPassword);

                }
                else
                {
                    Console.WriteLine("les 2 mdp ne sont pas identiques");
                    TempData["verifPwdIdentique"] = 0;
                }
            }
            else
            {
                Console.WriteLine("old password not matching");
                TempData["verifOldPwd"] = 0;
            }
            return RedirectToAction(actionName:"InfoPerso",controllerName:"Intern");
        }

        public IActionResult UpdateTelNum (BluePrintUserInfo data)
        {
            Console.WriteLine(data.Telephone);
            var SignedUser = _context.Etudiants.Where(su => su.IdEtudiant == HttpContext.Session.GetInt32("id")).FirstOrDefault();
            if (SignedUser != null)
            {
                if (data.Telephone.Length==10)
                {
                    SignedUser.Telephone = data.Telephone;
                    _context.SaveChanges(true);
                    HttpContext.Session.SetString("telephone", data.Telephone);
                    TempData["changesSaved"] = 1;
                }
                else
                {
                    TempData["WrongNumber"] = 1;
                }
                
                

            }
            return RedirectToAction(actionName: "InfoPerso", controllerName: "Intern");
        }
    }

    
}
