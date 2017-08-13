using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
//using System.Web.Mvc;

namespace SportsStore.Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue=false)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę produktu")]
        public string Name { get; set; }


        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage ="Proszę podać opis")]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być liczbą dodatnia")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Proszę określić kategorię")]
        public string Category { get; set; }

        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue= false)]
        public string ImageMimeType { get; set; }

    }
}
