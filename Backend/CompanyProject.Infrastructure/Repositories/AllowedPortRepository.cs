using CompanyProject.Application.Interfaces;
using CompanyProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyProject.Infrastructure.Repositories
{
    public class AllowedPortRepository : IAllowedPortRepository
    {
        private readonly ApplicationDbContext _context;

        public AllowedPortRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int?> GetAllowedPortAsync()
        {
            var port = await _context.AllowedPorts.FirstOrDefaultAsync();

            return port?.Port;
        }
    }

}
