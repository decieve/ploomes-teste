using ploomes_teste.persistence.Contexts;
using ploomes_teste.persistence.Contracts;

namespace ploomes_teste.persistence.Implementations
{
    public class LugarRepository : GeneralRepository,ILugarRepository
    {
        
        public LugarRepository(PloomesContext ploomesContext) : base(ploomesContext)
        {
        }
    }
}