using System;
using System.Collections.Generic;
using System.Text;

namespace UnionTask_Context.DBContext
{
  public  class User
    {

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Otp { get; set; }
        public string FcmToken { get; set; }
        public bool? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
