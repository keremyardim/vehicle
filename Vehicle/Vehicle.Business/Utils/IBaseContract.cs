using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle.Business.Utils
{
    public interface IBaseContract<T, E, Key>
        where T : class
        where E : class
        where Key : struct
    {
        Task AddRecord(E NewRecord);
        Task UpdateRecord(E UpdateRecord);
        Task DeleteRecord(Key RecordId);
        Task<T> GetRecordInfo(Key RecordId);
        Task<E> GetEditRecordInfo(Key RecordId);
        Task<PaginatedList<T>> GetRecords(string SortOrder, string CurrentFilter, string SearchString, int? Page);
        Task<List<T>> GetRecords();
        Task<List<SelectListItem>> GetRecordsForDropdown(int? SelectedID = null);

    }
}
