﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebCore.Common.Attributes;
using WebCore.Common.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.Controllers
{

    [ApiClassGroup(E_ApiGroup.User,"api/[controller]")]
    public class UserController : Controller
    {
        /// <summary>
        /// 测试类Get
        /// </summary>
        /// <returns></returns>
        [ApiMethodGroup( E_ApiGroup.User,"[action]",E_HttpVerbs.GET, E_HttpVerbs.POST)]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 通过id获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="value"></param>
        [ApiMethodGroup( E_ApiGroup.User,"AddUser", E_HttpVerbs.POST)]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
