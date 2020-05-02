using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model
{
    [Table("HizmetAlan")]
    public class HizmetAlan
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HizmetAlanId { get; set; }
        [Required,StringLength(50)]
        public string Baslik { get; set; }
        [Required]
        public string ResimURL { get; set; }
        [Required]
        public string Aciklama { get; set; }
    }
}