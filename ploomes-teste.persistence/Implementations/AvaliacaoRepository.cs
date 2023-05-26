using ploomes_teste.persistence.Contexts;
using ploomes_teste.persistence.Contracts;

namespace ploomes_teste.persistence.Implementations
{
    public class AvaliacaoRepository : GeneralRepository,IAvaliacaoRepository
    {
         public AvaliacaoRepository(PloomesContext ploomesContext) : base(ploomesContext)
        {
        }

    }
}