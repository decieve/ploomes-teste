using System.ComponentModel.DataAnnotations;
using ploomes_teste.domain;
namespace ploomes_teste.domain{
  public class TipoLugar
  {
  
    [Key]
    public short Id { get; set; }

    [Required, MaxLength(20)]
    public string Nome { get; set; }
    public virtual ICollection<Lugar> Lugares{get;set;}
  }

public enum TipoLugarEnum
{
  Restaurante = 1,
  Academia = 2,
  Bar = 3,
  Igreja =4,
  Oficina = 5,
  Escola =6,
  Mercearia = 7
}
}
