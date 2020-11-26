﻿using Invoices.EF;
using Invoices.Models;
using Invoices.Services;
using Invoices.TrackingPlugin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Invoices.Tests
{
    public class TrackingServicieFake : ITrackingService
    {
        private readonly IAsyncEnumerable<WorkItemRecord> _records;

        public TrackingServicieFake(IAsyncEnumerable<WorkItemRecord> records)
        {
            _records = records;
        }

        public async IAsyncEnumerable<WorkItemRecord> GetWorkItemsAsync(string QueryId)
        {
            await foreach (var item in _records)
            {
                yield return item;
            }
        }
    }
}
