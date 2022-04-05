using MapsterCodeGenerating;
using MapsterCodeGenerating.Application;
using MapsterCodeGenerating.Application.Dtos;

namespace MapsterCodeGenerating.Application
{
    public partial class EntityMapper : IEntityMapper
    {
        public EntityDto MapToDto(Entity p1)
        {
            return p1 == null ? null : new EntityDto()
            {
                Id = p1.Id,
                Description = p1.Description
            };
        }
    }
}