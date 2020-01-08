using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Viternus.Data.Interface;

namespace Viternus.Data.Repository
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class, IEntity, new()
    {
        #region Properties

        protected abstract ObjectQuery<T> EntityQuery { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the specific Entity by Id
        /// </summary>
        /// <returns></returns>
        public virtual T GetById(Guid rowId)
        {
            var results = from e in EntityQuery
                          where e.Id == rowId
                          select e;
            
            return results.FirstOrDefault();
        }

        /// <summary>
        /// Returns all Entities of this type
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetAll()
        {
            return EntityQuery.ToList();
        }

        /// <summary>
        /// Deletes an object from the database.
        /// </summary>
        /// <param name="objectToDelete"></param>
        public virtual void Delete(T objectToDelete)
        {
            DataConnector.Context.DeleteObject(objectToDelete);
            Save();
        }

        /// <summary>
        /// Returns a new object of this type, initializing the Id with a default Guid value
        /// </summary>
        /// <returns></returns>
        public virtual T New()
        {
            return New(Guid.NewGuid());
        }

        /// <summary>
        /// Returns a new object of this type, initializing the Id with Guid parameter passed-in
        /// </summary>
        /// <returns></returns>
        public virtual T New(Guid id)
        {
            T newObject = new T();

            //Go ahead and initialize the Id.  
            newObject.Id = id;
            return newObject;
        }

        /// <summary>
        /// Saves the specified Entity (and creates it as a new one if necessary)
        /// </summary>
        /// <param name="objectToSave"></param>
        public virtual void Save(T objectToSave)
        {
            if (!(EntityQuery.Any(c => c.Id == objectToSave.Id)))
            {
                DataConnector.Context.AddObject(objectToSave.GetType().Name, objectToSave);
            }

            Save();
        }

        /// <summary>
        /// Saves all changes that have been made using the Entity Framework
        /// </summary>
        public virtual void Save()
        {
            DataConnector.Context.SaveChanges();
        }

        #endregion


    }
}
