using System;

namespace Sphinx.Domain
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}