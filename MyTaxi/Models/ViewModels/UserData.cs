using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTaxi.Models.ViewModels
{
    public abstract class UserData
    {
        #region UserData For HeaderSet
        public bool isAuthorize { get; set; } = false;
        public string userName { get; set; }
        public string userSurname { get; set; }
        public string userPatronymic { get; set; }
        public bool isDriver { get; set; }
        #endregion
    }
}
