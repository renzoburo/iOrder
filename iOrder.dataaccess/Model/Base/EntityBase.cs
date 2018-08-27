#region meta
// iOrder - iOrder.dataaccess - EntityBase.cs
// 
// created [2018.08.15 17:59:55]
// changed [2018.08.15 17:59:56]
#endregion

namespace iOrder.dataaccess.Model.Base
{
    using System;
    using System.Collections.Generic;
    using Attributes;
    using Interfaces;

    public abstract class EntityBase : IEntity
    {
        [PrimaryKey]
        [Direction(DirectionType.InputOutput)]
        public Guid? Id { get; set; }

        public abstract IEnumerable<IEntity> Get();

        public abstract IEntity Get(Guid id);

        public abstract IEntity Save();

        public abstract bool Delete();
    }
}