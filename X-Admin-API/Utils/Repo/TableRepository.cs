using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using X_Admin_API.Models.DB;

using System.Threading.Tasks;
using X_Admin_API.Helper;

using System.Net;
using X_Admin_API.Models.DTO;
using X_Admin_API.Models.DTO.Table;

namespace X_Admin_API.Repository.Repo
{
    public class TableRepository
    {
        private THEntities db = null;

        public TableRepository()
        {
            db = new THEntities();
        }

        //-> create
        public async Task<TableViewDTO> Create(TableNewDTO newDTO)
        {
            newDTO = StringHelper.TrimStringProperties(newDTO);
            var checkTableGroup = await db.tblTableGroups.FirstOrDefaultAsync(x => x.tblg_Deleted == null && x.tblg_TableGroupID == newDTO.tabl_TableGroupID); // check whether itemgroup name exist or not 
            if (checkTableGroup == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "Table group not exist");

            var checkTableName = await db.tblTables.FirstOrDefaultAsync(r => r.tabl_Deleted == null && r.tabl_Name == newDTO.tabl_Name); // check whether itemgroup name exist or not
            if (checkTableName != null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "This table name already exsits");

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    
                    var record = (tblTable)Helper.Helper.MapDTOToDBClass<TableNewDTO, tblTable>(newDTO, new tblTable());
                    record.tabl_CreatedDate = DateTime.Now;
                   
                    db.tblTables.Add(record);
                    await db.SaveChangesAsync();
                    
                    transaction.Commit();

                    return await SelectByID(record.tabl_TableID);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        //-> SelectByID
        public async Task<TableViewDTO> SelectByID(int id)
        {
            /*
            var items = await (from i in db.tblItems
                               join g in db.tblItemGroups.Where(x => x.itmg_Deleted == null)
                                    on i.itemGroupID equals g.id
                               join d in db.sm_doc.Where(x => x.docs_Deleted == null && x.tableID == Helper.Helper.document_ItemTableID)
                                    on i.id.ToString() equals d.value into document
                               where i.item_Deleted == null && i.id == id
                               orderby i.name ascending
                               select new { item = i, document = document, itemGroup = g }
                            ).ToListAsync();
             
            if (items.Count == 0)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");

            var itemGroupView = new ItemGroupBase();
            itemGroupView = Helper.Helper.MapDBClassToDTO<tblItemGroup, ItemGroupBase>(items[0].itemGroup);
            var itemView = new ItemViewDTO();
            itemView = Helper.Helper.MapDBClassToDTO<tblItem, ItemViewDTO>(items[0].item);
            itemView.documents = Helper.Helper.GetDocuments(items[0].document.ToList());
            itemView.itemGroup = itemGroupView;

            return itemView;
            */

            
            
             //--i want use like this, but seem getting error with ayn
            var record = await db.tblTables.FirstOrDefaultAsync(r => r.tabl_Deleted == null && r.tabl_TableID == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");

            var view = new TableViewDTO();
            view = MappingHelper.MapDBClassToDTO<tblTable, TableViewDTO>(record);
            view.tableGroup = await new TableGroupRepository().SelectByID(int.Parse(record.tabl_TableGroupID.ToString()));
            //itemView = MappingHelper.MapDBClassToDTO<tblItem, ItemViewDTO>(item); //if map at last like this , document & and item group will be null
            return view;
            
        }

        //-> Edit
        public async Task<TableViewDTO> Edit(TableEditDTO editDTO)
        {
            editDTO = StringHelper.TrimStringProperties(editDTO);
            var record = db.tblTables.FirstOrDefault(r => r.tabl_Deleted == null && r.tabl_TableID == editDTO.tabl_TableID);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "This record has been deleted");

            var checkTableGroup = await db.tblTableGroups.FirstOrDefaultAsync(x => x.tblg_Deleted == null && x.tblg_TableGroupID == editDTO.tabl_TableGroupID); // check whether itemgroup name exist or not 
            if (checkTableGroup == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "Table group not exists");

            
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                   
                    record = (tblTable)Helper.Helper.MapDTOToDBClass<TableEditDTO, tblTable>(editDTO, record);
                    record.tabl_UpdatedDate = DateTime.Now;
                    db.Entry(record).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    transaction.Commit();
                    return await SelectByID(record.tabl_TableID);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }

        }

        //-> Delete
        public async Task<bool> Delete(int id)
        {
            var record = await db.tblTables.FirstOrDefaultAsync(x => x.tabl_Deleted == null && x.tabl_TableID == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            record.tabl_Deleted = 1;
            await db.SaveChangesAsync();
            return true;
        }

        //-> GetList
        public async Task<GetListDTO<TableViewDTO>> GetList(int currentPage)
        {
            var records = from i in db.tblTables
                                        where i.tabl_Deleted == null
                                        orderby i.tabl_Name ascending
                                        select i;
            return await Listing(currentPage, records);
        }

        //-> Search
        public async Task<GetListDTO<TableViewDTO>> Search(int currentPage, string search)
        {
            var records = from i in db.tblTables
                                        where i.tabl_Deleted == null && (i.tabl_Name.Contains(search))
                                        orderby i.tabl_Name ascending
                                        select i;
            return await Listing(currentPage, records, search);
        }

        

        //*** private method ***/
        private async Task<GetListDTO<TableViewDTO>> Listing(int currentPage, IQueryable<tblTable> records, string search = null)
        {
            int startRow = Helper.Helper.GetStartRow(currentPage);
            var myList = new List<TableViewDTO>();
            var myRecords = records.Skip(startRow).Take(Helper.Helper.pageSize);
            foreach (var item in myRecords)
            {
                myList.Add(await SelectByID(item.tabl_TableID));
            }
            var getList = new GetListDTO<TableViewDTO>();
            getList.metaData = await Helper.Helper.GetMetaData(currentPage, records, "name", "asc", search);
            getList.results = myList;
            return getList;
        }

        private async Task SaveItemToWarehouses(tblItem item)
        {
            var wareHouses = await db.tblWarehouses.Where(r => r.ware_Deleted == null).ToListAsync();
            foreach (var wareHouse in wareHouses)
            {
                var itemWareHouse = new tblItemWarehouse();
                itemWareHouse.itemID = item.id;
                itemWareHouse.wareHouseID = wareHouse.id;
                itemWareHouse.quantity = 0;
                itemWareHouse.itwh_CreatedDate = DateTime.Now;
                db.tblItemWarehouses.Add(itemWareHouse);
                db.SaveChanges();
            }
        }
    }
}