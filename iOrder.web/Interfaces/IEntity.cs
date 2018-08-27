#region meta
// iOrder - iOrder.dataaccess - IEntity.cs
// 
// created [2018.08.15 18:00:48]
// changed [2018.08.15 18:00:49]
#endregion

namespace iOrder.web.Interfaces
{
    using System;
    using System.Runtime.Serialization;

    public interface IEntityDTO
    {
        Guid Id { get; set; }
    }
}