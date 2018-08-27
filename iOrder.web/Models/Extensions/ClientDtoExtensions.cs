namespace iOrder.web.Models.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using dataaccess.Model;
    using DTO;

    public static class ClientDtoExtensions
    {
        public static Client MapToClient(this ClientDTO source)
        {
            if (source == null) return null;

            return new Client
            {
                Id = source.Id.IsNotNullOrEmpty() ? new Guid(source.Id) : (Guid?)null,
                FirstNames = source.FirstNames,
                Surname = source.Surname,
                AddressType = source.AddressType.IsNotNullOrEmpty() ? new Guid(source.AddressType) : (Guid?)null,
                StreetAddress = source.StreetAddress,
                Suburb = source.Suburb,
                City = source.City,
                PostalCode = Convert.ToInt32(source.PostalCode)
            };
        }

        public static ClientDTO MapToClientDto(this Client source)
        {
            if (source == null) return null;

            return new ClientDTO
            {
                Id = source.Id.ToString(),
                FirstNames = source.FirstNames,
                Surname = source.Surname,
                AddressType = source.AddressType.ToString(),
                StreetAddress = source.StreetAddress,
                Suburb = source.Suburb,
                City = source.City,
                PostalCode = source.PostalCode.ToString()
            };
        }

        public static IEnumerable<Client> MapToClientList(this IEnumerable<ClientDTO> source)
        {
            return source?.Select(MapToClient).ToList();
        }

        public static IEnumerable<ClientDTO> MapToClientDtoList(this IEnumerable<Client> source)
        {
            return source?.Select(MapToClientDto).ToList();
        }
    }
}
