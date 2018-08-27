namespace iOrder.web.Controllers
{
    using System;
    using System.Collections.Generic;
    using dataaccess.Model;
    using Microsoft.AspNetCore.Mvc;
    using Models.DTO;
    using Models.Extensions;

    [Route("api/AddressType")]
    public class AddressTypeController : Controller
    {
        AddressType addressTypeDataSource;

        private AddressType AddressTypeDataSource => addressTypeDataSource ?? (addressTypeDataSource = new AddressType());

        public IEnumerable<AddressTypeDTO> Get()
        {
            var addressType = (IEnumerable<AddressType>)AddressTypeDataSource.Get();
            return addressType.MapToAddressTypeDtoList();
        }

        public AddressTypeDTO Get(Guid id)
        {
            var addressType = (AddressType)AddressTypeDataSource.Get(id);
            return addressType.MapToAddessTypeDto();
        }
    }
}