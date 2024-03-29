﻿using Microsoft.AspNetCore.Mvc;
using warriorTime.BluePrintForm;
using System.Diagnostics;
using warriorTime.Models;
using Microsoft.AspNetCore.Http;

namespace warriorTime.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly WarriorTimeContext _context;
/**/
        // le nom du controler doit avoir le meme nom que le dossier 
        // chaque methode doit porter le meme nom que la vue 

        public LoginController(ILogger<LoginController> logger,WarriorTimeContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult LoginPage()
        {
            return View();
        }
        public IActionResult Register()
        {
            Console.WriteLine("j'affiche la page register");
            return View();
        }

        public IActionResult ForgetPassword()
        {
            Console.WriteLine("j'affiche la page forget password");
            return View();
        }

        public IActionResult SayHello()
        {
            Console.WriteLine("j'affiche la page say hello");
            return View();

        }

        public RedirectToActionResult VerifyCredentials(BluePrintLogIn data)
        {

            /*Console.WriteLine("vous etes co");
            Console.WriteLine("votre login est : "+data.Email);
            Console.WriteLine("votre pass est : " +data.Password);*/
            if (data.userType.Equals("student"))
            {
                var student = _context.Etudiants.Where(studentCredentials => studentCredentials.Email.Equals(data.Email) && studentCredentials.Mdp.Equals(data.Password)).FirstOrDefault();
                /*grace a pomeloo " _context.Etudiants.Where " va me generate une requete */
                /* studentCredentials est une variable temporaire qui prendra successivement une ligne depuis ma table etudiant*/
                if (student != null)
                {
                    /*to do: on ouvre la session */
                    HttpContext.Session.SetString("nom", student.Nom);
                    HttpContext.Session.SetString("prenom", student.Prenom);
                    HttpContext.Session.SetString("email", student.Email);
                    HttpContext.Session.SetString("telephone", student.Telephone);
                    HttpContext.Session.SetInt32("id", student.IdEtudiant);
                    HttpContext.Session.SetString("pwd", student.Mdp);
                    /*on redirige le user vers la page de dashboard*/
                    return RedirectToAction(actionName: "DashBoard", controllerName: "Intern");
                }
            }
            else
            {
                var coach = _context.Coaches.Where(coachCredentials => coachCredentials.Mail.Equals(data.Email) && coachCredentials.Mdp.Equals(data.Password)).FirstOrDefault();
                if (coach != null) {
                    HttpContext.Session.SetString("nom", coach.Nom);
                    HttpContext.Session.SetString("prenom",coach.Prenom);
                    HttpContext.Session.SetString("email",coach.Mail);
                    HttpContext.Session.SetString("telephone",coach.Tel);
                    HttpContext.Session.SetInt32("id",coach.IdCoach);
                    HttpContext.Session.SetString("pwd",coach.Mdp);

                    return RedirectToAction(actionName: "DashBoardCoach", controllerName: "Intern");
                }
               
            }
            
         
            TempData["signInFailed"] = 1;
            return RedirectToAction(actionName: "LoginPage", controllerName: "Login");

            /*Console.WriteLine(student.Nom); */
        }

        public void ForgotPass(BluePrintForgotPass data )
        {
            Console.WriteLine("aie aie aie");
            Console.WriteLine(data.Mail);
            var verif = _context.Etudiants.Where(verif => verif.Email.Equals(data.Mail)).FirstOrDefault();
/* il faut verifier que l'adresse mail du user existe bel et bien en base */
            if (verif != null) {
/*to do: il faut implementer l'envoi du mail pour re-initialiser le MDP */
                Console.WriteLine("existe");
            }
            else { 
                Console.WriteLine("non reconnu");
/*ici il faut prevenir le user que l'adresse mail n'existe pas dans la database*/
            }



        }
        public RedirectToActionResult SignOut()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("nom")))
            {
                HttpContext.Session.Clear();
                
            }

            return RedirectToAction(actionName: "LoginPage", controllerName: "Login");
        }
        // a chque fois que je click vers un bouton elle me renvoit vers une methode

        public IActionResult RegisterInfo(BluePrintSignUp data)
        {
            /*
                        Console.WriteLine(data.phoneNumber);
                        Console.WriteLine(data.name);
                        Console.WriteLine(data.mail);
                        Console.WriteLine(data.password);
                        Console.WriteLine(data.surname);*/

            try {
                var newStudent = new Etudiant(); /*newStuden generate une ligne vide dans la table etudiant*/
                newStudent.Nom = data.surname;
                /*la ligne 119 va recuperer les donnees du form (data.name) pour les inserer dans la table (colonne nom)*/
                newStudent.Prenom = data.name;
                newStudent.Email = data.mail;
                newStudent.Telephone = data.phoneNumber;
                newStudent.Mdp = data.password;

                _context.Etudiants.Add(newStudent);
                var status = _context.SaveChanges();/* va commit mes valeurs dans la table c'est comme appuyer sur entrer*/
                /*apres preparation de la ligne ,le _context va inserer mes valeurs dans la base de donnees */
                
                
                TempData["insertStatus"] = 1;
                return RedirectToAction(actionName: "LoginPage", controllerName: "Login");
                
            }catch(Exception ex) {


                TempData["insertStatus"] = 0;
                TempData["errorMessage"]= ex.Message.ToString();
                return RedirectToAction(actionName: "Register", controllerName: "Login");

            }

           
        }

        

    }
}