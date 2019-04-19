cg-upload组件使用方法
通过<cg-upload>标签对组件进行渲染，<cg-upload>在此写上传按钮名称</cg-upload>
常用属性如下：
id:组件的唯一id
auto:是否自动上传
mode:获取数据的形式 new只获取最新的，从数据库加载进来的不获取 all获取全部
params:上传时要带的参数
multiple:多文件上传
maxlength: 限制大小，0不限制，单位/kb
ext: 限制扩展名
actionul: 上传url
actionload:加载数据url
actionrm: 移除url
actiondl: 下载url
msg_rmfile: Function//删除时的提示，要返回true或false

通过添加属性ref=“ref名” 然后可以通过Vue的实例.$refs.ref名.getdata() 获取上传的数据集合