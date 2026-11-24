using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TemperaturStyring.Models
{
    public class BottleRepositoryDatabaseEF
    {
        private readonly ThermometerDbContext _context;
        public BottleRepositoryDatabaseEF(ThermometerDbContext context)
        {
            _context = context;
        }
    }    

}
