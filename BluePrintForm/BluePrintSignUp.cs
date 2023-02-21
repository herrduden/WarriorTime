using Mysqlx.Crud;

namespace warriorTime.BluePrintForm
{

/* squelette de mon formulaire qui va contenir la data que le user va saisir*/
    public class BluePrintSignUp
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string mail { get; set; }
        public string phoneNumber { get; set; }
        public string password { get; set; }

    }
}


