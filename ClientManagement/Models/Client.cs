using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace ClientManagement.Models
{
    public class Client
    {
        [Required(ErrorMessage="The ID field is required")]
        public int id { get; set; }
        [Required(ErrorMessage = "The Name field is required")]
        public string name { get; set; }
        [Required(ErrorMessage="Company is Required")]
        public string company { get; set; }
        public string gender { get; set; }
        public string designation { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        [Required(ErrorMessage = "Department is Required")]
        public string department { get; set; }
        public DateTime dob { get; set; }
        [Required(ErrorMessage = "The email address is required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string email { set; get; }
        public byte[] imagedata { get; set; }
    }
}