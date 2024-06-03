using DataAccess.Context;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Enums;
using SharedLibrary.Models;
using SharedLibrary.Utilities;

namespace DataAccess.Services
{
    public class DBService : IDBService
    {
        public readonly SalesOrderDBContext _salesOrderDBContext;

        public DBService(SalesOrderDBContext salesOrderDBContext)
        {
            _salesOrderDBContext = salesOrderDBContext;
        }

        public List<OrderHeader> SelectOrderHeader(Guid? id = null)
        {
            return _salesOrderDBContext.OrderHeader.Include(x => x.OrderLine).WhereIf(id != null, x => x.Id == id).ToList();
        }

        public int UpdateOrderHeader(OrderHeader orderHeader)
        {
            var result = _salesOrderDBContext.Update(orderHeader);
            var success = _salesOrderDBContext.SaveChangesAsync().Result;
            return 1;
        }

        public int DeleteOrderHeader(OrderHeader? orderHeader)
        {
            if (orderHeader != null)
            {
                var result = _salesOrderDBContext.OrderHeader.Remove(orderHeader);
                var success = _salesOrderDBContext.SaveChangesAsync().Result;
            }
            return 1;
        }

        public int InsertOrderHeader()
        {
            return 1;
        }

        public List<OrderHeader> FilterOrderHeader(OrderType filter)
        {
            return _salesOrderDBContext.OrderHeader.Include(x => x.OrderLine).Where(x => x.
                OrderType == filter).ToList();
        }
    }
}
