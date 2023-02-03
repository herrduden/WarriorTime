using Microsoft.AspNetCore.Mvc;
using warriorTime.BluePrintForm;
using System.Diagnostics;
using warriorTime.Models;

namespace warriorTime.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly WarriorTimeContext _context;
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

        public void VerifyCredentials(BluePrintLogIn data)
        {

            Console.WriteLine("vous etes co");
            Console.WriteLine("votre login est : "+data.Email);
            Console.WriteLine("votre pass est : " +data.Password);
            
            var student = _context.Etudiants.Where(studentCredentials => studentCredentials.Email.Equals(data.Email) && studentCredentials.Mdp.Equals(data.Password)).FirstOrDefault();
           /* studentCredentials est une variable temporaire qui prendra successivement une ligne depuis ma table etudiant*/
            
            Console.WriteLine(student.Nom); 
        }

        // a chque fois que je click vers un bouton elle me renvoit vers une methode



    }
}