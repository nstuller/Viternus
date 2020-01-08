using System;
namespace Viternus.Data.Interface
{
    interface IRepository<T>
     where T : class, Viternus.Data.Interface.IEntity, new()
    {
        void Delete(T objectToDelete);
        System.Collections.Generic.List<T> GetAll();
        T GetById(Guid rowId);
        T New();
        void Save();
        void Save(T objectToSave);
    }
}
