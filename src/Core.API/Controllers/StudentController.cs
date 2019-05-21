using Core.Common.Messages;
using Core.Entity;
using Core.IService;
using Microsoft.AspNetCore.Mvc;

namespace Core.API.Controllers
{
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpPost]
        public ResponseMessageWrap<int> Insert([FromBody]Student student)
        {
            return new ResponseMessageWrap<int>
            {
                Body = _studentService.Add(student)
            };
        }
        //[HttpPost]
        //public ResponseMessageWrap<int> DeleteById([FromBody]int id)
        //{
        //    return new ResponseMessageWrap<int>
        //    {
        //        Body = _studentService.DeleteById(id)
        //    };
        //}
        //[HttpPost]
        //public ResponseMessageWrap<int> Update([FromBody]ReportView reportView)
        //{
        //    return new ResponseMessageWrap<int>
        //    {
        //        Body = _studentService.Update(reportView)
        //    };
        //}

        //[HttpPost]
        //public ResponseMessageWrap<StudentViewModel> GetById([FromBody]int id)
        //{
        //    var reportView = _studentService.GetById(id);
        //    return new ResponseMessageWrap<ReportView>
        //    {
        //        Body = reportView
        //    };
        //}
        //[HttpPost]
        //public ResponseMessageWrap<QueryResponse<StudentViewModel>> Query([FromBody]QueryRequest reqMsg)
        //{
        //    var list = _studentService.Query(reqMsg);
        //    return new ResponseMessageWrap<QueryResponse<ReportView>>
        //    {
        //        Body = new QueryResponse<ReportView>
        //        {
        //            List = list
        //        }
        //    };
        //}
        //[HttpPost]
        //public ResponseMessageWrap<QueryByPageResponse<StudentViewModel>> QueryByPage([FromBody]QueryByPageRequest reqMsg)
        //{
        //    var total = _studentService.GetRecord(reqMsg);
        //    var list = _studentService.QueryByPage(reqMsg);
        //    return new ResponseMessageWrap<QueryByPageResponse<ReportView>>
        //    {
        //        Body = new QueryByPageResponse<ReportView>
        //        {
        //            Total = total,
        //            List = list
        //        }
        //    };
        //}
    }
}
