using SharedLibrary.Models;

namespace SharedLibrary.Interfaces
{
    public interface IXMLDBService
    {
        List<OrderHeader> SelectOrders();
        public int UpdateOrders(List<OrderHeader> orders);
    }
}
