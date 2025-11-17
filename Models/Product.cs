using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models {
    [Table("Products")]
    public class Product {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required(ErrorMessage = "O nome é Obrigatório")]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O preço precisa ser maior que 0")]

        [Column(TypeName = "decimal(18,2)")]

        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "A quantidade em estoque precisa ser maior que 0")]
        public int Stock { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "A categoria é obrigatória")]
        public string Category { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; } = DateTime.Now;

    }
}
