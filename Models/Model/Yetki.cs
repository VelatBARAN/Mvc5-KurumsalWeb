using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model
{
    [Table("Yetki")]
    public class Yetki
    {
        [Key]
        public int YetkiId { get; set; }
        [Required]
        public string YetkiAd { get; set; }
        public ICollection<Admin> Admins { get; set; }
    }
}