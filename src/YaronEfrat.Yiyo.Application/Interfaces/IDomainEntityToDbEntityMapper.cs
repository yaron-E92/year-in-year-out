using YaronEfrat.Yiyo.Domain.Reflection.Models;

namespace YaronEfrat.Yiyo.Application.Interfaces;

public interface IDomainEntityToDbEntityMapper<TDomainEntity, TDbEntity>
    where TDbEntity : IDbEntity where TDomainEntity : Entity
{
     void Map(TDomainEntity domainEntity, TDbEntity existingDbEntity);

     public void MapMany(IList<TDomainEntity> domainEntities, IList<TDbEntity> existingDbEntities)
     {
         for (int i = 0; i < domainEntities.Count; i++)
         {
            Map(domainEntities[i], existingDbEntities[i]);
         }
     }
}
