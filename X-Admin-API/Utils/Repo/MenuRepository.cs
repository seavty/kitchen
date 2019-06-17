using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using X_Admin_API.Helper;
using X_Admin_API.Models.DB;
using X_Admin_API.Models.DTO.Menu;

namespace X_Admin_API.Utils.Repo
{
    public class MenuRepository
    {
        private THEntities db = null;

        public MenuRepository()
        {
            db = new THEntities();
        }


        //-> create
        public async Task<MenuViewDTO> Create(MenuNewDTO newDTO)
        {
            newDTO = StringHelper.TrimStringProperties(newDTO);
            var checkMenuGroup = await db.tblMenus.FirstOrDefaultAsync(x => x.menu_Deleted == null && x.menu_MenuGroupID == newDTO.menu_MenuGroupID); // check whether itemgroup name exist or not 
            if (checkMenuGroup == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "Menu group not exist");

            var checkMenuName = await db.tblMenus.FirstOrDefaultAsync(r => r.menu_Deleted == null && r.menu_Name == newDTO.menu_Name); // check whether itemgroup name exist or not
            if (checkMenuName != null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "Menu name already exsits");

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var record = (tblMenu)Helper.Helper.MapDTOToDBClass<MenuNewDTO, tblMenu>(newDTO, new tblMenu());
                    record.menu_CreatedDate = DateTime.Now;

                    db.tblMenus.Add(record);
                    await db.SaveChangesAsync();

                    transaction.Commit();

                    return await SelectByID(record.menu_MenuID);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        //-> SelectByID
        public async Task<MenuViewDTO> SelectByID(int id)
        {
            
            //--i want use like this, but seem getting error with ayn
            var record = await db.tblMenus.FirstOrDefaultAsync(r => r.menu_Deleted == null && r.menu_MenuID == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");

            var view = new MenuViewDTO();
            view = MappingHelper.MapDBClassToDTO<tblTable, MenuViewDTO>(record);
            view.menuGroup = await new MenuGroupRepository().SelectByID(int.Parse(record.menu_MenuGroupID.ToString()));
            //itemView = MappingHelper.MapDBClassToDTO<tblItem, ItemViewDTO>(item); //if map at last like this , document & and item group will be null
            return view;

        }

        //-> Edit
        public async Task<MenuViewDTO> Edit(MenuEditDTO editDTO)
        {
            editDTO = StringHelper.TrimStringProperties(editDTO);
            var record = db.tblTables.FirstOrDefault(r => r.tabl_Deleted == null && r.tabl_TableID == editDTO.menu_MenuGroupID);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "This record has been deleted");

            var checkTableGroup = await db.tblTableGroups.FirstOrDefaultAsync(x => x.tblg_Deleted == null && x.tblg_TableGroupID == editDTO.menu_MenuID); // check whether itemgroup name exist or not 
            if (checkTableGroup == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "Table group not exists");


            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {

                    record = (tblTable)Helper.Helper.MapDTOToDBClass<MenuEditDTO, tblTable>(editDTO, record);
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




    }
}