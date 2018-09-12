namespace iOrder.web.Controllers
{
    using System;
    using System.Collections.Generic;
    using dataaccess.Model;
    using Microsoft.AspNetCore.Mvc;
    using Models.DTO;
    using Models.Extensions;

    public class ClientController : Controller
    {
        Client clientDataSource;
        AddressType addressTypeDataSource;

        private Client ClientDataSource => clientDataSource ?? (clientDataSource = new Client());

        private AddressType AddressTypeDataSource => addressTypeDataSource ?? (addressTypeDataSource = new AddressType());

        [Route("api/GetClients")]
        public IEnumerable<ClientDTO> Get()
        {
            try
            {
                var clients = (IEnumerable<Client>)ClientDataSource.Get();
                return clients.MapToClientDtoList();
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        [Route("api/GetClient")]
        public ClientDTO Get(Guid clientId)
        {
            try
            {
                if (clientId == Guid.Empty) return null;
                var client = (Client)ClientDataSource.Get(clientId);
                return client.MapToClientDto();
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("api/SaveClient")]
        public IActionResult SaveClient([FromBody]ClientDTO clientDto)
        {
            try
            {
                var client = clientDto.MapToClient();
                var result = client.Save();
                return Ok(result);
            }
            catch (Exception exception)
            {
                return NotFound();
            }
        }

        [Route("api/DeleteClient")]
        public IActionResult Delete([FromBody]ClientDTO clientDto)
        {
            try
            {
                var client = clientDto.MapToClient();
                var result = client.Delete();
                return Ok(result);
            }
            catch (Exception exception)
            {
                return NotFound();
            }
        }

        [Route("api/GetAddressTypes")]
        public IEnumerable<AddressTypeDTO> GetAddressTypes()
        {
            try
            {
                var addressTypes = (IEnumerable<AddressType>)AddressTypeDataSource.Get();
                return addressTypes.MapToAddressTypeDtoList();
            }
            catch (Exception exception)
            {
                return null;
            }
        }
    }
}