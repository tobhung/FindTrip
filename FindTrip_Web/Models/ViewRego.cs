using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FindTrip_Web.Models
{
    public class ViewRego
    {
        [Required(ErrorMessage = "{0}Must fill in")]//
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }

        [Required(ErrorMessage = "{0}Must fill in")]//
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}