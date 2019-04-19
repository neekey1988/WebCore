using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore.Component.Options
{
    public class OptionsUpload : IOptions<OptionsUpload>
    {
        private long maxLength;
        /// <summary>
        /// 最大长度。单位kb1M=1024kb
        /// </summary>
        public long MaxLength { get {
                return maxLength * 1024;
            }
            set {
                maxLength = value;
            } }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string[] Extension { get; set; }
        /// <summary>
        /// 匹配路由
        /// </summary>
        public string Router { get; set; }
        /// <summary>
        /// 程序集名，反射用
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// 类名,程序集下的除了程序集名之外的完整路径的类名，即包含目录的类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 保存在数据库的相对路径，以/开头，可选以下参数
        /// $root:根目录
        /// $yyyy$MM$dd等 时间
        /// $filename 文件名
        /// $guid GUID
        /// $ticks Ticks时间戳
        /// $ext 扩展名
        /// $dirname 提交时要传递dirname参数
        /// </summary>
        public string Path { get; set; }

        private string root;
        /// <summary>
        /// 设置根目录，比如d:/project/  数据库就只保存path的相对路径， 如果附件是上传到项目根目录下可设置为空，否则保存在其他位置，就要设置这个，以防止服务器迁移,不支持参数
        /// </summary>
        public string Root {
            get {
                return root?.TrimEnd('/').TrimEnd('\\');
            }
            set {
                root = value;
            }
        }

        /// <summary>
        /// 由前端传的参数，如果Path里面还有这个变量，则前端就必须传入这个参数
        /// </summary>
        public string DirectoryName { get; set; }

        /// <summary>
        /// 要调用的方法名
        /// </summary>
        public string HandleName { get; set; }
        /// <summary>
        /// 预留验证账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 预留验证密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 预留密钥
        /// </summary>
        public string Key { get; set; }

        public OptionsUpload Value => this;
    }
}
