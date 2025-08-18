using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models {

    [Table("Sales")]
    public class Sale {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O cliente é obrigatório")]
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        [Required]
        public DateTime SaleDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "A forma de pagamento é obrigatória")]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        public ICollection<SaleItem> Items { get; set; }

    }
}
