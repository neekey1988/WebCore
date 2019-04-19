<template>
    <div class="cgUpload clearfix">
        <div class="uploadbtnBox clearfix">
            <input type="file" :id="id" style="display:none" :multiple="multiple"  v-on:change="addfile"/>
            <button type="button" class="btn btn-primary btn-sm" v-on:click="selectfile">
                <span class="glyphicon glyphicon-folder-open" aria-hidden="true"></span>  <slot></slot>
            </button>
        </div>
        <cg-upload-item v-for="item in files" :width="info_width" v-bind="item" @uploadfile="uploadfile"></cg-upload-item>
    </div>
</template>
<script>
    import cgUploadItem from "./cgUploadItem.vue";
    import md5 from "../md5.js";
    import _xhr from "./xhr.js";
//todo:缓存上传了但没提交的数据，再次进入页面时重新加载缓存的数据;

    export default {
        name: 'cgUpload',
        created: function () {
            _xhr.uploadProgress = this.callProgress;
            _xhr.uploadTimeOut = this.callTimeOut;
            _xhr.uploadComplete = this.callComplete;
            _xhr.uploadError = this.callError;
            this.loaddata();
        },
        props: {
            id: {
                type: String,
                required: true
            },
            auto: {//自动上传
                type: Boolean,
                default:true
            },
            mode: {//获取数据的形式 new all
                type: String,
                default:"new"
            },
            params: String,//上传时要带的参数
            multiple: {//多文件上传
                type: Boolean,
            },
            maxlength: {//kb
                type: Number,
                default:0
            },
            ext: {//扩展名
                type: String,
                default:""
            },
            actionul: {//上传url
                type: String,
                default: ""
            },
            actionload: {//加载数据url
                type: String,
                default: ""
            },
            actionrm: {//移除url
                type: String,
                default: ""
            },
            actiondl: {//下载url
                type: String,
                default:""
            },
            msg_rmfile: Function//删除时的提示，要返回true或false
        },
        components: {
            cgUploadItem
        },
        data:function() {
            return {
                files: [],
                info_width: "calc(100% - 105px)",
                fileinfo: function (dbid,file, filename, path, percent,status, isonline) {
                    return {
                        "md5": md5.build(),
                        "dbid": dbid,
                        "file": file,
                        "filename": filename,
                        "path": path,
                        "percent": percent,
                        "status": status,
                        "isonline":isonline//是否线上获取，线上获取的在提交数据时不会提交到服务器
                    }
                }
            }
        },
        methods: {
            selectfile: function (e) {
                if (this.actionul == "") {
                    throw "未设置cg-upload组件的actionul属性，组件已禁用";
                    console.log("未设置cg-upload组件的actionul属性，组件已禁用");
                    return;
                }
                if (this.multiple == "" && this.files.length == 1) {
                    return;
                }
                document.querySelector("#" + this.id).click();
            },
            getdata: function () {
                var list = new Array();
                for (var i = 0; i < this.files.length; i++) {
                    //已在线上的文件并且获取方式是获取新增的
                    if (this.mode=="new"&&this.files[i].isonline) {
                        continue;
                    }

                    if (this.files[i].status!="success") {
                        continue;
                    }
                    list.push({
                        "dbid": this.files[i].dbid,
                        "path": this.files[i].path,
                        "filename": this.files[i].filename,
                    });
                }
                return list;
            },
            loaddata: function () {
                var that = this;
                if (that.actionload == "") {
                    return;
                }
                axios({
                    method: 'get',
                    url: that.actionload
                }).then(function (resp) {
                    if (typeof resp.data != "object")
                        return;
                    var data = resp.data;
                    for (var i = 0; i < data.length; i++) {
                        that.files.push(that.fileinfo(data[i].dbid, null, data[i].filename, data[i].path, "100", "success", true));
                    }
                }).catch(function (resp) {
                    console.log(resp.message);
                });
            },
            checkfile: function (_file) {
                if (parseInt(this.maxlength)!=0&&_file.file.size > parseInt( this.maxlength) * 1024) {
                    _file.status = "failed";
                    _file.percent = "上传失败:上传的文件超过系统所限制的大小";
                    return;
                }
                var _ext = this.ext;
                if (_ext != "" && _ext.indexOf(_file.file.name.split('.')[1])<0) {
                    _file.status = "failed";
                    _file.percent = "上传失败:上传的文件类型不符合要求";
                    return;
                }
            },
            addfile: function () {
                var list_file = document.querySelector("#" + this.id).files;
                //file控件在添加到this.files列表后，就会手动清空掉，此时又会调用change事件，所以这里判断下
                if (list_file.length == 0) {
                    return;
                }

                for (var i = 0; i < list_file.length; i++) {
                    var file = list_file[i];
                    var existsfile = false;
                    for (var j = 0; j < this.files.length; j++) {
                        if (this.files[j].filename == file.name) {
                            existsfile = true;
                        }
                    }
                    if (existsfile) 
                        continue;
                    var fi = this.fileinfo(null, file, file.name, "", "0", "ready", false);
                    this.checkfile(fi);
                    this.files.push(fi);
                }

                //只有通过:绑定的prop才能转成boolean，否则都是string
                if (this.auto+""=="true") {
                    this.send();
                }
                document.getElementById(this.id).value = '';
            },
            callProgress: function (e, evt) {
                e.status = "uploading";
                e.percent = Math.round(evt.loaded * 100 / evt.total).toFixed(2);
            },
            callTimeOut: function (e, ev) {
                e.status = "failed";
                e.percent = "上传失败，连接超时";
            },
            callError: function (e, ev) {//该事件只在网络异常时才触发，404之类的错误都时在xhr.onload事件里面
                e.status = "failed";
                e.percent = "上传失败，请确认上传服务是否正常";
            },
            callComplete: function (e, ev) {
                if ((ev.target.status >= 200 && ev.target.status < 300) || ev.target.status == 304) {                    var result = ev.target.response;                    if (typeof ev.target.response == "string")                        result=JSON.parse(ev.target.response);                    e.status = result.status?"success": "failed";
                    if (result.status) {
                        e.path = result.data[0].path;
                        e.filename = result.data[0].filename;
                        e.dbid = result.data[0].dbid;
                    }
                    else {
                        e.percent = result.msg;
                    }
                }                else {
                    e.status = "failed";
                    e.percent =  "上传失败:("+ev.target.status + ")" + ev.target.statusText;
                }            },
            uploadfile: function (md5) {
                for (var i = 0; i < this.files.length; i++) {
                    if (this.files[i].md5 == md5) {
                        _xhr.submit(this.files[i], this.actionul, this.params);
                        break;
                    }
                }
            },
            send: function () {
                for (var i = 0; i < this.files.length; i++) {
                    if (this.files[i].status != "ready")
                        continue;
                    _xhr.submit(this.files[i], this.actionul, this.params);
                }
            }
        },
        computed: {
            att_rmultiple: function () {
                return this.multiple == "" ? "" : "multiple";
            }
        },
        watch: {
            files: function () {
                if (this.files.length <= 1) {
                    this.info_width = "calc(100% - 105px)";
                }
                else {
                    this.info_width = "calc(100% - 15px)";
                }
            }
        }
    }
</script>

<style scoped>
.cgUpload {
    width: 100%;
    margin-top: 10px;
    height: 30px;
}
.cgUpload .uploadbtnBox
{
    float: left;
    height: 30px;
    width: 80px;
    margin-right: 8px;
}


.clearfix:after {
    content: ".";
    display: block;
    height: 0;
    visibility: hidden;
    clear: both;
}

.clearfix {
    _zoom: 1;
}

.clearfix {
    *zoom: 1;
}
</style>
