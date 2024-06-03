using SharedLibrary.Enums;
using SharedLibrary.Models;

namespace DataAccess.Interfaces
{
    public interface IDBService
    {
        public List<OrderHeader> SelectOrderHeader(Guid? id = null);
        public List<OrderHeader> FilterOrderHeader(OrderType filter);
        public int UpdateOrderHeader(OrderHeader orderHeader);
        public int DeleteOrderHeader(OrderHeader? orderHeader);
        public int InsertOrderHeader();
    }
}
