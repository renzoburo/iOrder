namespace iOrder.web.Models.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using dataaccess.Interfaces;
    using dataaccess.Model;
    using DTO;
    using Interfaces;

    public static class OrderProductDtoExtensions
    {
        public static OrderProduct MapToOrderProduct(this OrderProductDTO source)
        {
            if (source == null) return null;

            return new OrderProduct
            {
                Id = source.Id.IsNotNullOrEmpty() ? new Guid(source.Id) : (Guid?)null,
                OrderId = source.OrderId.IsNotNullOrEmpty() ? new Guid(source.OrderId) : (Guid?)null,
                ProductId = source.ProductId.IsNotNullOrEmpty() ? new Guid(source.ProductId) : (Guid?)null,
                Quantity = Convert.ToInt32(source.Quantity)
            };
        }

        public static OrderProductDTO MapToOrderProductDto(this OrderProduct source)
        {
            if (source == null) return null;

            return new OrderProductDTO
            {
                Id = source.Id.ToString(),
                OrderId = source.OrderId.ToString(),
                ProductId = source.ProductId.ToString(),
                Quantity = source.Quantity.ToString()
            };
        }

        public static IEnumerable<OrderProduct> MapToOrderProductList(this IEnumerable<OrderProductDTO> source)
        {
            return source?.Select(MapToOrderProduct).ToList();
        }

        public static IEnumerable<OrderProductDTO> MapToOrderProductDtoList(this IEnumerable<OrderProduct> source)
        {
            return source?.Select(MapToOrderProductDto).ToList();
        }
    }
}
