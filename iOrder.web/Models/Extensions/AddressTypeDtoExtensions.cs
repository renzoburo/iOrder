namespace iOrder.web.Models.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using dataaccess.Model;
    using DTO;
    using Interfaces;

    public static class AddressTypeDtoExtensions
    {
        public static AddressType MapToAddressType(this AddressTypeDTO source)
        {
            if (source == null) return null;

            return new AddressType
            {
                Id = source.Id.IsNotNullOrEmpty() ? new Guid(source.Id) : (Guid?)null,
                AddressTypeName = source.AddressTypeName
            };
        }

        public static AddressTypeDTO MapToAddessTypeDto(this AddressType source)
        {
            if (source == null) return null;

            return new AddressTypeDTO
            {
                Id = source.Id.ToString(),
                AddressTypeName = source.AddressTypeName
            };
        }

        public static IEnumerable<AddressType> MapToAddressTypeList(this IEnumerable<AddressTypeDTO> source)
        {
            return source?.Select(MapToAddressType).ToList();
        }

        public static IEnumerable<AddressTypeDTO> MapToAddressTypeDtoList(this IEnumerable<AddressType> source)
        {
            return source?.Select(MapToAddessTypeDto).ToList();
        }
    }
}
