using Core.Common;
using Core.Entity;
using Core.IService;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Core.API.Controllers
{
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentService _studentService;
        public StudentController(ILogger<StudentController> logger,
            IStudentService studentService)
        {
            _logger = logger;
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

        [Route("add"),HttpPost]
        public async Task<ResponseMessageWrap<bool>> Add([FromBody]StudentModel model)
        {
            return new ResponseMessageWrap<bool>
            {
                Body = await _studentService.Add(model)
            };
        }

        [Route("update"),HttpPut]
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

        [Route("delete-by-id"),HttpDelete]
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
