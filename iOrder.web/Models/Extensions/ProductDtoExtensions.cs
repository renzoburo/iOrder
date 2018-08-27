namespace iOrder.web.Models.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using dataaccess.Interfaces;
    using dataaccess.Model;
    using DTO;
    using Interfaces;

    public static class ProductDtoExtensions
    {
        public static Product MapToProduct(this ProductDTO source)
        {
            if (source == null) return null;

            return new Product
            {
                Id = source.Id.IsNotNullOrEmpty() ? new Guid(source.Id) : (Guid?)null,
                Title = source.Title,
                Description = source.Description,
                Price = decimal.Parse(source.Price, NumberStyles.Any, CultureInfo.InvariantCulture),
                Image = source.Image
            };
        }

        public static ProductDTO MapToProductDto(this Product source)
        {
            if (source == null) return null;

            return new ProductDTO
            {
                Id = source.Id.ToString(),
                Title = source.Title,
                Description = source.Description,
                Price = source.Price.ToString(CultureInfo.InvariantCulture),
                Image = source.Image
            };
        }

        public static IEnumerable<Product> MapToProductList(this IEnumerable<ProductDTO> source)
        {
            return source?.Select(MapToProduct).ToList();
        }

        public static IEnumerable<ProductDTO> MapToProductDtoList(this IEnumerable<Product> source)
        {
            return source?.Select(MapToProductDto).ToList();
        }
    }
}
