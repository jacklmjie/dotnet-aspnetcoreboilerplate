using Core.API.Options;
using Core.Common;
using Core.IService;
using Core.Models;
using Core.Models.Identity.Entity;
using Core.Repository.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Core.API.Controllers
{
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private JwtOption _jwtOption;
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly IStudentService _studentService;
        public StudentController(ILogger<StudentController> logger,
            IOptions<JwtOption> jwtOption,
            IUnitOfWorkFactory unitOfWorkFactory,
            IStudentService studentService)
        {
            _logger = logger;
            _jwtOption = jwtOption.Value;
            _uowFactory = unitOfWorkFactory;
            _studentService = studentService;
        }

        [Route("query-by-id"), HttpGet]
        public async Task<ResponseMessageWrap<Student>> QueryById(long id)
        {
            var student = await _studentService.Get(id);
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
        public async Task<ResponseMessageWrap<bool>> Add([FromBody]StudentDto dto)
        {
            using (var uow = _uowFactory.Create())
            {
                var id = await _studentService.Add(dto);
                uow.SaveChanges();
            }

            return new ResponseMessageWrap<bool>
            {
                Body = true
            };
        }

        [Route("update"), HttpPut]
        public async Task<ResponseMessageWrap<bool>> Update(long id, Student entity)
        {
            if (id != entity.Id)
            {
                throw new APIException("400", $"值不匹配id[{id}]");
            }
            return new ResponseMessageWrap<bool>
            {
                Body = await _studentService.Update(entity)
            };
        }

        [Route("delete-by-id"), HttpDelete]
        public async Task<ResponseMessageWrap<bool>> DeleteById(long id)
        {
            var student = await _studentService.Get(id);
            if (student == null)
            {
                throw new APIException("404", $"未查询到数据id[{id}]");
            }
            return new ResponseMessageWrap<bool>
            {
                Body = await _studentService.Delete(student)
            };
        }

        [Route("query-by-page")]
        [HttpPost]
        public async Task<ResponseMessageWrap<QueryResponseByPage<Student>>> QueryByPage([FromBody]QueryRequestByPage reqMsg)
        {
            var pages = await _studentService.GetListPaged(reqMsg);
            return new ResponseMessageWrap<QueryResponseByPage<Student>>
            {
                Body = pages
            };
        }
    }
}
