using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore.Component.Options
{
    public class OptionsSwagger : IOptions<OptionsSwagger>
    {
        /// <summary>
        /// xml说明文件的名称，ide通过webapi注释生产的xml说明，需要在项目——属性——生成——勾选“xml文档文件”
        /// </summary>
        public string XmlCommentsName { get; set; }
        public Dictionary<string, Swashbuckle.AspNetCore.Swagger.Info> SwaggerDocs { get; set; }
        public OptionsSwagger Value => this;
    }
}
