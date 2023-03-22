﻿namespace Domain.Base;

public class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
}