using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using X_Admin_API.Models.DB;
using X_Admin_API.Models.DTO.Customer;
using System.Threading.Tasks;
using X_Admin_API.Helper;
using System.Web;
using System.Net;
using X_Admin_API.Models.DTO;
using X_Admin_API.Models.DTO.TableGroup;

namespace X_Admin_API.Repository
{
    public class TableGroupRepository
    {
        private THEntities db = null;

        public TableGroupRepository()
        {
            db = new THEntities();
        }

        //-> Create
        public async Task<TableGroupViewDTO> Create(TableGroupNewDTO newDTO)
        {
            newDTO = StringHelper.TrimStringProperties(newDTO);
            var checkName = await db.tblTableGroups.FirstOrDefaultAsync(x => x.tblg_Deleted == null && x.tblg_Name == newDTO.tblg_Name); // check whether itemgroup name exist or not
            if (checkName != null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "This table group already exsits.");

            var record = (tblTableGroup)Helper.Helper.MapDTOToDBClass<TableGroupNewDTO, tblTableGroup>(newDTO, new tblTableGroup());
            record.tblg_CreatedDate = DateTime.Now;
            db.tblTableGroups.Add(record);
            await db.SaveChangesAsync();
            db.Entry(record).Reload();
            return await SelectByID(record.tblg_TableGroupID);
        }

        //-> SelectByID
        public async Task<TableGroupViewDTO> SelectByID(int id)
        {
            var record = await db.tblTableGroups.FirstOrDefaultAsync(r => r.tblg_Deleted == null && r.tblg_TableGroupID == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            return Helper.Helper.MapDBClassToDTO<tblItemGroup, TableGroupViewDTO>(record);
        }

        //-> Edit
        public async Task<TableGroupViewDTO> Edit(TableGroupEditDTO editDTO)
        {
            editDTO = StringHelper.TrimStringProperties(editDTO);

            var record = await db.tblTableGroups.FirstOrDefaultAsync(r => r.tblg_Deleted == null && r.tblg_TableGroupID == editDTO.tblg_TableGroupID);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "This record has been deleted");

            var checkName = await db.tblTableGroups.FirstOrDefaultAsync(x => x.tblg_Deleted == null && x.tblg_Name == editDTO.tblg_Name && x.tblg_TableGroupID != editDTO.tblg_TableGroupID);
            if (checkName != null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "This table group already exsits");

            record = (tblTableGroup)Helper.Helper.MapDTOToDBClass<TableGroupEditDTO, tblTableGroup>(editDTO, record);
            record.tblg_UpdatedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return await SelectByID(record.tblg_TableGroupID);
        }

        //-> Delete
        public async Task<Boolean> Delete(int id)
        {
            var record = await db.tblTableGroups.FirstOrDefaultAsync(r => r.tblg_Deleted == null && r.tblg_TableGroupID == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            record.tblg_Deleted = 1;
            await db.SaveChangesAsync();
            return true;
        }

        //-> GetList
        public async Task<GetListDTO<TableGroupViewDTO>> GetList(int currentPage)
        {
            var records = from r in db.tblTableGroups
                                                where r.tblg_Deleted == null
                                                orderby r.tblg_Name ascending
                                                select r;
            return await Listing(currentPage, records);
        }

        //-> Search
        public async Task<GetListDTO<TableGroupViewDTO>> Search(int currentPage, string search)
        {
            var records = from r in db.tblTableGroups
                                                where r.tblg_Deleted == null && (r.tblg_Name.Contains(search))
                                                orderby r.tblg_Name ascending
                                                select r;
            return await Listing(currentPage, records, search);
        }

        //-- private method --//
        private async Task<GetListDTO<TableGroupViewDTO>> Listing(int currentPage, IQueryable<tblTableGroup> records, string search = null)
        {
            int startRow = Helper.Helper.GetStartRow(currentPage);
            var myList = new List<TableGroupViewDTO>();
            var myRecords = await records.Skip(startRow).Take(Helper.Helper.pageSize).ToListAsync();
            foreach (var record in myRecords)
            {
                myList.Add(await SelectByID(record.tblg_TableGroupID));
            }
            var getList = new GetListDTO<TableGroupViewDTO>();
            getList.metaData = await Helper.Helper.GetMetaData(currentPage, records, "name", "asc", search);
            getList.results = myList;
            return getList;
        }
    }
}