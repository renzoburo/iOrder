namespace iOrder.web.Models.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using dataaccess.Model;
    using DTO;

    public static class OrderDtoExtensions
    {
        public static Order MapToOrder(this OrderDTO source)
        {
            if (source == null) return null;

            return new Order
            {
                Id = source.Id.IsNotNullOrEmpty() ? new Guid(source.Id) : (Guid?)null,
                ClientId = source.ClientId.IsNotNullOrEmpty() ? new Guid(source.ClientId) : (Guid?)null,
                Complete = source.Complete,
                DateCreated = source.DateCreated,
                DateUpdated = source.DateUpdated
            };
        }

        public static OrderDTO MapTOrderDto(this Order source)
        {
            if (source == null) return null;

            return new OrderDTO
            {
                Id = source.Id.ToString(),
                ClientId = source.ClientId.ToString(),
                Complete = source.Complete,
                DateCreated = source.DateCreated,
                DateUpdated = source.DateUpdated
            };
        }

        public static IEnumerable<Order> MapToOrderList(this IEnumerable<OrderDTO> source)
        {
            return source?.Select(MapToOrder).ToList();
        }

        public static IEnumerable<OrderDTO> MapToOrderDtoList(this IEnumerable<Order> source)
        {
            return source?.Select(MapTOrderDto).ToList();
        }
    }
}
