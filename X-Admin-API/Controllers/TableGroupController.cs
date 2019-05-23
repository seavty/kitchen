using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using X_Admin_API.Models.DTO;
using X_Admin_API.Models.DTO.Customer;
using X_Admin_API.Models.DTO.TableGroup;
using X_Admin_API.Repository;

namespace X_Admin_API.Controllers
{
    public class TableGroupController : ApiController
    {
        private const string route = Helper.Helper.apiVersion + "tablegroups";
        private const string routeWithConstraint = route + "/{id:int:min(1)}";
        private TableGroupRepository repository = null;
        
        public TableGroupController()
        {
            repository = new TableGroupRepository();
        }

       
        [HttpPost]
        [Route(route)]
        [ResponseType(typeof(TableGroupViewDTO))]
        public async Task<IHttpActionResult> Create(TableGroupNewDTO record)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(await repository.Create(record));
            }
            catch (HttpException ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
        [HttpGet]
        [Route(routeWithConstraint)]
        [ResponseType(typeof(TableGroupViewDTO))]
        public async Task<IHttpActionResult> SelectByID(int id)
        {
            try
            {
                return Ok(await repository.SelectByID(id));
            }
            catch (HttpException)
            {
                return NotFound();
            }
        }

         
        [HttpPut]
        [Route(routeWithConstraint)]
        [ResponseType(typeof(TableGroupViewDTO))]
        public async Task<IHttpActionResult> Edit(int id, [FromBody] TableGroupEditDTO record)
        {
            try
            {
                if (id != record.tblg_TableGroupID)
                    return BadRequest("Invalid id ");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(await repository.Edit(record));
            }
            catch (HttpException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route(routeWithConstraint)]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                if (await repository.Delete(id))
                    return Ok();
                return NotFound();
            }
            catch (HttpException)
            {
                return NotFound();
            }
        }

        
        [HttpGet]
        [Route(route)]
        [ResponseType(typeof(GetListDTO<TableGroupViewDTO>))]
        public async Task<IHttpActionResult> Get([FromUri] int currentPage)
        {
            return Ok(await repository.GetList(currentPage));
        }

        
        [HttpGet]
        [Route(route)]
        [ResponseType(typeof(GetListDTO<TableGroupViewDTO>))]
        public async Task<IHttpActionResult> Search([FromUri] int currentPage, [FromUri] string search)
        {
            return Ok(await repository.Search(currentPage, search));
        }
    }
}
