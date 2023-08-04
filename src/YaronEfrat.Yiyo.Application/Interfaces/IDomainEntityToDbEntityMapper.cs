using YaronEfrat.Yiyo.Domain.Reflection.Models;

namespace YaronEfrat.Yiyo.Application.Interfaces;

public interface IDomainEntityToDbEntityMapper<in TDomainEntity, in TDbEntity> where   TDbEntity : IDbEntity where  TDomainEntity : Entity
{
     void Map(TDomainEntity domainEntity, TDbEntity existingDbEntity);
}
