#region meta
// iOrder - iOrder.dataaccess - EntityBase.cs
// 
// created [2018.08.15 17:59:55]
// changed [2018.08.15 17:59:56]
#endregion

namespace iOrder.web.Models.DTO.Base
{
    using System;
    using System.Runtime.Serialization;
    using Interfaces;

    [DataContract]
    public abstract class EntityDTOBase : IEntityDTO
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }
    }
}