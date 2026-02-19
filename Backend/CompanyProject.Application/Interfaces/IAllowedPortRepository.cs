using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyProject.Application.Interfaces
{
    public interface IAllowedPortRepository
    {
        Task<int?> GetAllowedPortAsync();
    }
}
