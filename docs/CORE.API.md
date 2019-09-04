#Web API 接口约定
GET请求 200返回值 404未找到
POST请求 200返回值 400验证未通过
Create[POST]请求 201重定向 400验证未通过
Delete[DELETE]请求 204无返回值 400验证未通过 404未找到
#HttpPatch表示部分更新,PUT发送整个更新实体
Update[HttpPatch]请求 204无返回值 400验证未通过 404未找到
Update[PUT]请求 204无返回值 400验证未通过 404未找到
Find[POST]请求 200返回值 404未找到