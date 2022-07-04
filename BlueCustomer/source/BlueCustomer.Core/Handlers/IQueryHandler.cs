﻿using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCustomer.Core.Handlers;

public interface IQueryHandler<T, TResult>
{
    Task<Result<TResult>> Handle(T query, CancellationToken cancellationToken);
}