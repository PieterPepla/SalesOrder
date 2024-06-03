using Microsoft.AspNetCore.Hosting;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces;
using SharedLibrary.Models;
using SharedLibrary.Utilities;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SharedLibrary.Services
{
    public class XMLDBService : IXMLDBService
    {
        private IHostingEnvironment _environment;

        public XDocument xdoc;

        public XMLDBService(IHostingEnvironment environment)
        {
            _environment = environment;
            xdoc = XDocument.Load(Path.Combine(_environment.WebRootPath, "XML", "DB.xml"), LoadOptions.PreserveWhitespace);
        }

        public List<OrderHeader> SelectOrders()
        {
            var orders = xdoc.Element("ArrayOfOrderHeader").Elements("OrderHeader").ToList();

            List<OrderHeader> ordersList = new List<OrderHeader>();

            foreach (var order in orders)
            {
                List<OrderLine> orderLines = new List<OrderLine>();

                foreach (var line in order.Element("OrderLine")?.Elements("OrderLine"))
                {
                    orderLines.Add(new OrderLine
                    {
                        Id = Guid.Parse(line.Element("Id")?.Value),
                        LineNumber = int.Parse(line.Element("LineNumber")?.Value),
                        ProductCode = line.Element("ProductCode")?.Value,
                        ProductType = (ProductType)Enum.Parse(typeof(ProductType), line.Element("ProductType")?.Value, true),
                        CostPrice = decimal.Parse(line.Element("CostPrice")?.Value),
                        SalesPrice = decimal.Parse(line.Element("SalesPrice")?.Value),
                        Quantity = int.Parse(line.Element("Quantity")?.Value),
                        DateCreated = DateTime.Parse(line.Element("DateCreated")?.Value),
                        DateUpdated = DateTime.Parse(line.Element("DateUpdated")?.Value)
                    });
                };

                ordersList.Add(new OrderHeader
                { 
                    Id = Guid.Parse(order.Element("Id")?.Value),
                    OrderNumber = order.Element("OrderNumber")?.Value,
                    OrderType = (OrderType)Enum.Parse(typeof(OrderType), order.Element("OrderType")?.Value, true),
                    OrderStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), order.Element("OrderStatus")?.Value, true),
                    CustomerName = order.Element("CustomerName")?.Value,
                    OrderLine = orderLines,
                    DateCreated = DateTime.Parse(order.Element("DateCreated")?.Value),
                    DateUpdated = DateTime.Parse(order.Element("DateUpdated")?.Value)
                });

            }
            return ordersList;
        }

        public int UpdateOrders(List<OrderHeader> orders)
        {
            string xml = XmlSerialize<List<OrderHeader>>.Serialize(orders);
            
            xdoc = XDocument.Parse(xml);

            xdoc.Save(Path.Combine(_environment.WebRootPath, "XML", "DB.xml"));

            return 1;
        }
    }
}
