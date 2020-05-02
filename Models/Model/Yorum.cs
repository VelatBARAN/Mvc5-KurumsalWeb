using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model
{
    [Table("Yorum")]
    public class Yorum
    {
        [Key]
        public int YorumId { get; set; }
        [Required]
        [DisplayName("Adı Soyadı")]
        public string AdSoyad { get; set; }
        [Required]
        [DisplayName("EPosta")]
        public string Eposta { get; set; }
        [Required]
        [DisplayName("İçerik")]
        public string Icerik { get; set; }
        [DisplayName("Onay")]
        public bool Onay { get; set; }
        [DisplayName("Yorum Tarihi")]
        public DateTime YorumTarih { get; set; }
        public int? BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}