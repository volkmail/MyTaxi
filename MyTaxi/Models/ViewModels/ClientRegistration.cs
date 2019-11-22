using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTaxi.Models.ViewModels
{
    public class ClientRegistration
    {
        public string login { get; set; }
        public string password { get; set; }
        public string userName { get; set; }
        public string userSurname { get; set; }
        public string userPatronymic { get; set; }
        public string userPhone { get; set; }

    }
}
