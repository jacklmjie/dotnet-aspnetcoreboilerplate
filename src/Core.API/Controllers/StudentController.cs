using Core.Common;
using Core.IContract;
using Core.IRepository;
using Core.Models;
using Core.Models.Identity.Entity;
using Core.Repository.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Core.API.Controllers
{
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly IStudentContract _studentContract;
        public StudentController(ILogger<StudentController> logger,
            IUnitOfWorkFactory unitOfWorkFactory,
            IStudentContract studentService)
        {
            _logger = logger;
            _uowFactory = unitOfWorkFactory;
            _studentContract = studentService;
        }

        [Route("query-by-id"), HttpGet]
        public async Task<ResponseMessageWrap<Student>> QueryById(long id)
        {
            var student = await _studentContract.Get(id);
            if (student == null)
            {
                throw new APIException("404", $"未查询到数据id[{id}]");
            }
            return new ResponseMessageWrap<Student>
            {
                Body = student
            };
        }

        [Route("add"), HttpPost]
        [Description("新增")]
        public async Task<ResponseMessage> Add([FromBody]StudentDto dto)
        {
            using (var uow = _uowFactory.Create())
            {
                var id = await _studentContract.Add(dto);
                uow.SaveChanges();
            }

            return new ResponseMessage
            {
                IsSuccess = true
            };
        }

        [Route("update"), HttpPut]
        public async Task<ResponseMessage> Update(long id, Student entity)
        {
            if (id != entity.Id)
            {
                throw new APIException("400", $"值不匹配id[{id}]");
            }
            return new ResponseMessage()
            {
                IsSuccess = await _studentContract.Update(entity)
            };
        }

        [Route("delete-by-id"), HttpDelete]
        public async Task<ResponseMessage> DeleteById(long id)
        {
            var student = await _studentContract.Get(id);
            if (student == null)
            {
                throw new APIException("404", $"未查询到数据id[{id}]");
            }
            return new ResponseMessage()
            {
                IsSuccess = await _studentContract.Delete(student)
            };
        }

        [Route("query-by-page")]
        [HttpPost]
        public async Task<ResponseMessageWrap<QueryResponseByPage<Student>>> QueryByPage([FromBody]QueryRequestByPage reqMsg)
        {
            var pages = await _studentContract.GetListPaged(reqMsg);
            return new ResponseMessageWrap<QueryResponseByPage<Student>>
            {
                Body = pages
            };
        }
    }
}
