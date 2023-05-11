using System.ComponentModel.DataAnnotations;
using App.Models;

namespace m01_Start.Controllers.Blog{
     public class CreatPostModels : Post{
          [Display(Name =  "Chuyen Muc")]
          public int[]? CategoryIDs{get; set; }
     }
}