#region meta
// iOrder - iOrder.dataaccess - IEntity.cs
// 
// created [2018.08.15 18:00:48]
// changed [2018.08.15 18:00:49]
#endregion

namespace iOrder.dataaccess.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Attributes;

    public interface IEntity
    {
        Guid? Id { get; set; }

        IEnumerable<IEntity> Get();

        IEntity Get(Guid id);

        IEntity Save();

        bool Delete();
    }
}