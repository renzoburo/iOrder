namespace iOrder.web.Models.DTO
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class BasketDTO
    {
        [DataMember(Name = "OrderInfo")]
        public OrderDTO OrderInfo { get; set; }

        [DataMember(Name = "OrderProducts")]
        public IEnumerable<OrderProductDTO> OrderProducts { get; set; }
    }
}
