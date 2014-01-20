using Coinage.Domain.Abstract.Entites;
using System;

namespace Coinage.Domain.Concrete.Entities
{
    public abstract class EditableEntity : Entity, ITimestamps
    {
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
