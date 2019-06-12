using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using X_Admin_API.Helper;
using X_Admin_API.Models.DB;
using X_Admin_API.Models.DTO;
using X_Admin_API.Models.DTO.MenuGroup;

namespace X_Admin_API.Utils.Repo
{
    public class MenuGroupRepository
    {
        private THEntities db = null;

        public MenuGroupRepository()
        {
            db = new THEntities();
        }


        //-> Create
        public async Task<MenuGroupViewDTO> Create(MenuGroupNewDTO newDTO)
        {
            newDTO = StringHelper.TrimStringProperties(newDTO);
            var checkName = await db.tblMenuGroups.FirstOrDefaultAsync(x => x.mnug_Deleted == null && x.mnug_Name == newDTO.mnug_Name); // check whether itemgroup name exist or not
            if (checkName != null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "This menu group already exsits.");

            var record = (tblMenuGroup)Helper.Helper.MapDTOToDBClass<MenuGroupNewDTO, tblMenuGroup>(newDTO, new tblMenuGroup());
            record.mnug_CreatedDate = DateTime.Now;
            db.tblMenuGroups.Add(record);
            await db.SaveChangesAsync();
            db.Entry(record).Reload();
            return await SelectByID(record.mnug_MenuGroupID);
        }


        //-> SelectByID
        public async Task<MenuGroupViewDTO> SelectByID(int id)
        {
            var record = await db.tblMenuGroups.FirstOrDefaultAsync(r => r.mnug_Deleted == null && r.mnug_MenuGroupID == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            return Helper.Helper.MapDBClassToDTO<tblMenuGroup, MenuGroupViewDTO>(record);
        }


        //-> Edit
        public async Task<MenuGroupViewDTO> Edit(MenuGroupEditDTO editDTO)
        {
            editDTO = StringHelper.TrimStringProperties(editDTO);

            var record = await db.tblMenuGroups.FirstOrDefaultAsync(r => r.mnug_Deleted == null && r.mnug_MenuGroupID == editDTO.mnug_MenuGroupID);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "This record has been deleted");

            var checkName = await db.tblMenuGroups.FirstOrDefaultAsync(x => x.mnug_Deleted == null && x.mnug_Name == editDTO.mnug_Name && x.mnug_MenuGroupID != editDTO.mnug_MenuGroupID);
            if (checkName != null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "This table group already exsits");

            record = (tblMenuGroup)Helper.Helper.MapDTOToDBClass<MenuGroupEditDTO, tblMenuGroup>(editDTO, record);
            record.mnug_UpdatedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return await SelectByID(record.mnug_MenuGroupID);
        }

        //-> Delete
        public async Task<Boolean> Delete(int id)
        {
            var record = await db.tblMenuGroups.FirstOrDefaultAsync(r => r.mnug_Deleted == null && r.mnug_MenuGroupID == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            record.mnug_Deleted = 1;
            await db.SaveChangesAsync();
            return true;
        }

        //-> GetList
        public async Task<GetListDTO<MenuGroupViewDTO>> GetList(int currentPage)
        {
            var records = from r in db.tblMenuGroups
                          where r.mnug_Deleted == null
                          orderby r.mnug_Name ascending
                          select r;
            return await Listing(currentPage, records);
        }

        //-> Search
        public async Task<GetListDTO<MenuGroupViewDTO>> Search(int currentPage, string search)
        {
            var records = from r in db.tblMenuGroups
                          where r.mnug_Deleted == null && (r.mnug_Name.Contains(search))
                          orderby r.mnug_Name ascending
                          select r;
            return await Listing(currentPage, records, search);
        }


        //-- private method --//
        private async Task<GetListDTO<MenuGroupViewDTO>> Listing(int currentPage, IQueryable<tblMenuGroup> records, string search = null)
        {
            int startRow = Helper.Helper.GetStartRow(currentPage);
            var myList = new List<MenuGroupViewDTO>();
            var myRecords = await records.Skip(startRow).Take(Helper.Helper.pageSize).ToListAsync();
            foreach (var record in myRecords)
            {
                myList.Add(await SelectByID(record.mnug_MenuGroupID));
            }
            var getList = new GetListDTO<MenuGroupViewDTO>();
            getList.metaData = await Helper.Helper.GetMetaData(currentPage, records, "name", "asc", search);
            getList.results = myList;
            return getList;
        }


    }
}