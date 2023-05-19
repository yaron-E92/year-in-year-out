﻿using System.ComponentModel.DataAnnotations.Schema;

using YaronEfrat.Yiyo.Application.Interfaces;

namespace YaronEfrat.Yiyo.Application.Models;

public class YearInEntity : IDbEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public ICollection<FeelingEntity> Feelings { get; set; }

    public MottoEntity Motto { get; set; }

    public ICollection<PersonalEventEntity> PersonalEvents { get; set; }

    public ICollection<WorldEventEntity> WorldEvents { get; set; }
}
