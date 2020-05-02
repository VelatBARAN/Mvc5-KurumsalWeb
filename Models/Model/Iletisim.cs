using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model
{
    [Table("Iletisim")]
    public class Iletisim
    {
        [Key]
        public int IletisimId { get; set; }
        [Required(ErrorMessage ="Adres giriniz")]
        public string Adres { get; set; }
        [Required,StringLength(50,ErrorMessage ="50 karakter olmalıdır")]
        public string Email { get; set; }
        [Required, StringLength(50, ErrorMessage = "50 karakter olmalıdır")]
        public string Telefon { get; set; }
        [StringLength(50, ErrorMessage = "50 karakter olmalıdır")]
        public string Fax { get; set; }
        [StringLength(50, ErrorMessage = "50 karakter olmalıdır")]
        public string Telegram { get; set; }
        [StringLength(50, ErrorMessage = "50 karakter olmalıdır")]
        public string Linkedln { get; set; }
        [StringLength(50, ErrorMessage = "50 karakter olmalıdır")]
        public string Twitter { get; set; }
        [StringLength(50, ErrorMessage = "50 karakter olmalıdır")]
        public string Instagram { get; set; }
    }
}