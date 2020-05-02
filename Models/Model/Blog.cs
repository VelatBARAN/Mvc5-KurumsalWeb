using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model
{
    [Table("Blog")]
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        [StringLength(100,ErrorMessage ="En az 5 karakter girilmelidir!",MinimumLength =5)]
        public string Baslik { get; set; }
        [Required(ErrorMessage ="Boş geçilemez")]
        public string Icerik { get; set; }
        [DisplayName("Blog Resim")]
        [Required]
        public string ResimURL { get; set; }
        [Required]
        public int? KategoriId { get; set; }
        public Kategori Kategori { get; set; }
        public ICollection<Yorum> Yorums { get; set; }
    }
}