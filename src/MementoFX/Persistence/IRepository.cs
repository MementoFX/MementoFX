using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento.Domain;

namespace Memento.Persistence
{
    /// <summary>
    /// Represents a repository capable of retrieving aggregates instance from a store 
    /// </summary>
    public interface IRepository 
    {
        /// <summary>
        /// Retrieves an aggregate instance
        /// </summary>
        /// <typeparam name="T">The aggregate type</typeparam>
        /// <param name="id">The aggregate Id</param>
        /// <returns>The aggregate instance</returns>
        T GetById<T>(Guid id) where T : IAggregate;

        /// <summary>
        /// Retrieve an aggregate instance restoring the value it would have along a given timeline
        /// </summary>
        /// <typeparam name="T">The aggregate type</typeparam>
        /// <param name="id">The aggregate Id</param>
        /// <param name="timelineId">The timeline Id</param>
        /// <returns>The aggregate instance</returns>
        T GetById<T>(Guid id, Guid timelineId) where T : IAggregate;

        /// <summary>
        /// Retrieve an aggregate instance restoring the value it had at a given point in time
        /// </summary>
        /// <typeparam name="T">The aggregate type</typeparam>
        /// <param name="id">The aggregate Id</param>
        /// <param name="pointInTime">A point in time</param>
        /// <returns>The aggregate instance</returns>
        T GetById<T>(Guid id, DateTime pointInTime) where T : IAggregate;

        /// <summary>
        /// Retrieve an aggregate instance restoring the value it would have in a given point in time happening along a timeline
        /// </summary>
        /// <typeparam name="T">The aggregate type</typeparam>
        /// <param name="id">The aggregate Id</param>
        /// <param name="timelineId">The timeline Id</param>
        /// <param name="pointInTime">A point in time</param>
        /// <returns>The aggregate instance</returns>
        T GetById<T>(Guid id, Guid timelineId, DateTime pointInTime) where T : IAggregate;

        /// <summary>
        /// Retrieve an aggregate instance restoring the value it had at a given point in time
        /// </summary>
        /// <typeparam name="T">The aggregate type</typeparam>
        /// <param name="id">The aggregate Id</param>
        /// <param name="pointsInTime">The list of dates to use to produce the series</param>
        /// <returns>The aggregate instance</returns>
        T[] GetSeriesById<T>(Guid id, IEnumerable<DateTime> pointsInTime) where T : IAggregate;

        /// <summary>
        /// Saves an aggregate instance
        /// </summary>
        /// <typeparam name="T">The aggregate's type</typeparam>
        /// <param name="item">The aggregate instance</param>
        void Save<T>(T item) where T : IAggregate;

        /// <summary>
        /// Saves an aggregate instance
        /// </summary>
        /// <typeparam name="T">The aggregate's type</typeparam>
        /// <param name="item">The aggregate instance</param>
        Task SaveAsync<T>(T item) where T : IAggregate;

        /// <summary>
        /// Saves a snapshot of an aggregate instance
        /// </summary>
        /// <typeparam name="T">The aggregate's type</typeparam>
        /// <param name="item">The aggregate instance</param>
        void SaveAndTakeSnapshot<T>(T item) where T : IAggregate, ISupportSnapshots;
    }
}
