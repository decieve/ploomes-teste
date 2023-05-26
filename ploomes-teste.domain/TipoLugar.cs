using System.ComponentModel.DataAnnotations;

public class TipoLugar
{
    [Key]
    public short Id { get; set; }

    [Required, MaxLength(20)]
    public string Nome { get; set; }
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