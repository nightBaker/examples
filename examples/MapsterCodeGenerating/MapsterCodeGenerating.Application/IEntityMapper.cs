using Mapster;
using MapsterCodeGenerating.Application.Dtos;

namespace MapsterCodeGenerating.Application
{
    [Mapper]
    public interface IEntityMapper
    {
        //map from POCO to DTO
        EntityDto MapToDto(Entity entity);
    }
}