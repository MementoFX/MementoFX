//using Memento.Domain;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Memento
//{
//    /// <summary>
//    /// Defines an event representing the snapshot of an aggregate
//    /// </summary>
//    /// <typeparam name="T">The type of the aggregate</typeparam>
//    public class SnapshotTakenEvent<T> : DomainEvent where T : IAggregate
//    {
//        /// <summary>
//        /// Gets or sets the shapshot
//        /// </summary>
//        public T Snapshot { get; private set; }

//        /// <summary>
//        /// Creates a snapshot instance
//        /// </summary>
//        /// <param name="instance">The aggregate instance to be used as the snapshot</param>
//        public SnapshotTakenEvent(T instance)
//        {
//            if (instance == null)
//                throw new ArgumentNullException(nameof(instance));

//            Snapshot = instance;
//        }
//    }
//}
