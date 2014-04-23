using System;

namespace Coinage.Domain.Entites
{
    public abstract class EditableEntity : Entity, ITimestamps
    {
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        protected EditableEntity()
        {
            CreatedOn = DateTime.Now;
        }
    }
}
