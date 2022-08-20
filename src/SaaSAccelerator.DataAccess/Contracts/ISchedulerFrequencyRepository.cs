﻿using System;
using System.Collections.Generic;
using Microsoft.Marketplace.SaaSAccelerator.DataAccess.Entities;

namespace Microsoft.Marketplace.SaaSAccelerator.DataAccess.Contracts
{
    /// <summary>
    /// Repository to access the possible data for Scheduler Frequency.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ISchedulerFrequencyRepository : IDisposable
    {
        /// <summary>
        /// Gets all Frequencies.
        /// </summary>
        /// <returns>List of all Scheduler Frequencies supported by the application.</returns>
        IEnumerable<SchedulerFrequency> GetAll();

        /// <summary>
        /// Gets Frequency  by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns> Frequency.</returns>
        SchedulerFrequency GetById(int id);

        
    }
}
