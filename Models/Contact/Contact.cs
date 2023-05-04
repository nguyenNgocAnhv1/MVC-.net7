using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace m01_Start.Models
{
     public class Contact{
          [Key]
          public int Id { get; set; }
          [Column(TypeName ="nvarchar")]
          [Required]
          [StringLength(100)]
          public string FullName { get; set;}
          [Required]
          [StringLength(100)]
          public string Email { get; set;}
          
          public DateTime DateSent{ get; set; }
          public string Message { get; set; }
          [StringLength(50)]
          public string? Phone { get; set; }
     }
}