using System.ComponentModel.DataAnnotations;

namespace Talabat.Api.Dtos.BasketDto
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]

        public string ProductName { get; set; }
        [Required]

        public string PictureUrl { get; set; }
        [Required]
        [Range(0.1,double.MaxValue, ErrorMessage = "price must be greater than 0")]
        public decimal Price { get; set; }
        [Required]

        public string Brand { get; set; }
        [Required]

        public string Category { get; set; }
        [Required]
        [Range(1, double.MaxValue,ErrorMessage ="Quantity must be  at least one")]
        public int Quantity { get; set; }
    }
}
