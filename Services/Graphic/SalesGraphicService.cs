using ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERP.Services.Graphic {
    public class SalesGraphicService {

        private readonly Context.AppDbContext _context;
        public SalesGraphicService(Context.AppDbContext context) {
            _context = context;
        }
        public List<SalesGraphic> GetSalesGraphic(int dias = 360) {
            var data = DateTime.Now.AddDays(-dias);

            var salesGraphic = _context.SaleItems
     .Where(si => si.Sale.SaleDate >= data)
     .Join(_context.Products,
           si => si.ProductId,
           p => p.Id,
           (si, p) => new { si, p })
     .GroupBy(x => x.p.Name)
     .Select(g => new SalesGraphic {
         ProductName = g.Key,
         Quantity = g.Sum(x => x.si.Quantity),
         Total = g.Sum(x => x.si.Quantity * x.si.UnitPrice)
     })
     .OrderByDescending(sg => sg.Total)
     .ToList();

            return salesGraphic;
        }

    }
}
