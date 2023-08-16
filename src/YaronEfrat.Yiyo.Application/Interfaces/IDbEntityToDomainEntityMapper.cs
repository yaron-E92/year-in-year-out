using YaronEfrat.Yiyo.Domain.Reflection.Models;

namespace YaronEfrat.Yiyo.Application.Interfaces;

public interface IDbEntityToDomainEntityMapper<in TDbEntity, out TDomainEntity> where   TDbEntity : IDbEntity where  TDomainEntity : Entity
{
    TDomainEntity Map(TDbEntity dbEntity);
}
