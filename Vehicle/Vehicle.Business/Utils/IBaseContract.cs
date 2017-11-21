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
        /// <summary>
        /// Add New Record to DB. Using parameter EditViewModel
        /// </summary>
        /// <param name="NewRecord"></param>
        /// <returns></returns>
        Task AddRecord(E NewRecord);
        /// <summary>
        /// Update current record. Using parameter EditViewModel
        /// </summary>
        /// <param name="UpdateRecord"></param>
        /// <returns></returns>
        Task UpdateRecord(E UpdateRecord);
        /// <summary>
        /// Delete Current Record. 
        /// </summary>
        /// <param name="RecordId"></param>
        /// <returns></returns>
        Task DeleteRecord(Key RecordId);
        /// <summary>
        /// Get spesific data whit key
        /// </summary>
        /// <param name="RecordId"></param>
        /// <returns></returns>
        Task<T> GetRecordInfo(Key RecordId);
        /// <summary>
        /// Get spesific data for editing
        /// </summary>
        /// <param name="RecordId"></param>
        /// <returns></returns>
        Task<E> GetEditRecordInfo(Key RecordId);
        /// <summary>
        /// Get result set from db with parameter
        /// </summary>
        /// <param name="SortOrder"></param>
        /// <param name="CurrentFilter"></param>
        /// <param name="SearchString"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        Task<PaginatedList<T>> GetRecords(string SortOrder, string CurrentFilter, string SearchString, int? Page);
        /// <summary>
        /// Get all record from db
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetRecords();
        /// <summary>
        /// Get all active record from db to select component data
        /// </summary>
        /// <param name="SelectedID"></param>
        /// <returns></returns>
        Task<List<SelectListItem>> GetRecordsForDropdown(int? SelectedID = null);

    }
}
