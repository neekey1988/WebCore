<template>
        <div class="uploadinfo" :style="{width:width}" :md5="md5">
            <div class="fileinfo"  @mouseenter ="ispointer = true" @mouseleave="ispointer = false">
                <div class="filename">
                    <a :href="get_action_dl" v-if="path!=''&&$parent.actiondl!=''">{{filename}}</a>
                    <span v-if="path==''||$parent.actiondl==''">{{filename}}</span>
                </div>
                <div class="uploadstate">
                    <span v-if="status=='failed'||isloading" class="failed">{{percent}}</span>
                    <span class="percent" v-if="status=='uploading'||(status=='ready'&&!ispointer)">{{percent}}%</span>
                    <span class="glyphicon glyphicon-ok-sign text-success" v-if="isloading?false:((status=='success'&&!ispointer)||(status=='success'&$parent.actionrm==''&&path!=''&&ispointer))"></span>
                    <span class="glyphicon glyphicon-remove-sign text-danger" v-if="isloading?false:((status=='failed'&&!ispointer)||(status=='failed'&&$parent.actionrm==''&&path!=''&&ispointer))"></span>
                    <transition name="anim-rm">
                        <span class="glyphicon glyphicon-remove text-danger pointer" v-if="isloading?false:((status!='uploading'&&ispointer&&$parent.actionrm!='')||(path==''&&(status=='failed'||status=='ready')&&ispointer))" @click="remove"></span>
                    </transition>
                    <transition name="anim-up">
                        <span class="glyphicon glyphicon-circle-arrow-up text-danger pointer" v-if="status=='ready'&&!auto&&ispointer" v-on:click="$emit('uploadfile',md5)"></span>
                    </transition>
                    <span class="glyphicon glyphicon-ban-circle text-warning" v-if="status!='uploading'&&isloading"></span>
                </div>
            </div>
            <div class="progress" style="height: 10px;">
                <div class="progress-bar progress-bar-primary" role="progressbar"
                     aria-valuenow="100" aria-valuemin="0" aria-valuemax="100"
                     :style="{width: percent+'%'}">
                    <!--<span class="sr-only">40% 完成</span>-->
                </div>
            </div>
        </div>
</template>

<script>
    export default {
        name: 'cgUploadItem',
        props: {
            md5: String,
            dbid:String,
            filename: String,
            path: String,
            status: String,
            auto: Boolean,
            percent: {
                type: String,
                default:"0"
            },
            width: String,
        },
        data: function(){
            return {
                ispointer: false,
                isloading: false
            }
        },
        methods: {
            remove: function (e) {
                if (this.$parent.msg_rmfile != undefined && !this.$parent.msg_rmfile(this.filename)) {
                    return;
                }
                var inum = -1;
                for (var i = 0; i < this.$parent.files.length; i++) {
                    if (this.$parent.files[i].md5 == this.md5) {
                        inum = i;
                        break;
                    }
                }
                if (inum == -1) 
                    return;
                var f = this.$parent.files[inum];
                //如果path为空，说明根本没上传，可能是文件过大之类的问题，这种清空下可直接删除
                if (f.path == "") {
                    this.$parent.files.splice(inum, 1);
                    return;
                }
                //path为空说明没有上传到服务器，就直接删除
                if (f.status == "failed" && f.path=="") {
                    this.$parent.files.splice(inum, 1);
                    return;
                }
                //如果是待上传状态，也直接删除
                if (f.status == "ready") {
                    this.$parent.files.splice(inum, 1);
                    return;
                }
                if (this.$parent.actionrm == undefined || this.$parent.actionrm == "") {
                    this.$parent.files[i].percent = "删除失败：没有配置actionrm属性";
                    this.$parent.files[i].status = "failed";
                    return;
                }
                this.isloading = true;
                this.$parent.files[inum].percent = "正在删除，请稍等...";
                this.removeFile(inum,f);
            },
            removeFile: function (i, f) {
                var that = this;
                axios({
                    method: 'get',
                    url: this.get_action_rm
                }).then(function (resp) {
                    that.isloading = false;
                    if ((resp.status >= 200 && resp.status < 300) || resp.status == 304) {
                        if (resp.data.status) {
                            that.$parent.files.splice(i, 1);
                        }
                        else {
                            that.$parent.files[i].percent = "删除失败："+resp.data.msg;
                            that.$parent.files[i].status = "failed";
                        }
                    }
                    else {
                        that.$parent.files[i].percent = "删除失败：(" + resp.status + ")" + resp.statusText;
                        that.$parent.files[i].status = "failed";
                    }

                }).catch(function (resp) {
                    that.isloading = false;
                    if (resp.response == undefined) {
                        that.$parent.files[i].percent = "删除失败：" + resp.message;
                        that.$parent.files[i].status = "failed";
                    }
                    else {
                        that.$parent.files[i].percent = "删除失败：(" + resp.response.status + ")" + resp.response.statusText;
                        that.$parent.files[i].status = "failed";
                    }
                });
            }
        },
        computed: {
            get_action_dl: function () {
                var url = this.$parent.actiondl;
                if (url.split('?').length==1) {
                    return url + "?dbid=" + this.dbid+"&filename=" + encodeURI(this.filename) + "&path=" + encodeURI(this.path);
                }
                else {
                    return url.replace("#dbid#", this.dbid).replace("#filename#", encodeURI(this.filename)).replace("#path#", encodeURI(this.path));
                }
            },
            get_action_rm: function () {
                var url = this.$parent.actionrm;
                if (url.split('?').length == 1) {
                    return url + "?dbid=" + this.dbid + "&filename=" + encodeURI(this.filename) + "&path=" + encodeURI(this.path);
                }
                else {
                    return url.replace("#dbid#", this.dbid).replace("#filename#", encodeURI(this.filename)).replace("#path#", encodeURI(this.path));
                }
            },
        }
    }
</script>

<style scoped>
.glyphicon {
    top:-1px;
}
.pointer {
    cursor: pointer;
}
.uploadinfo {
    float: left;
    margin-left: 5px;
    width: calc(100% - 105px);
}

.uploadinfo .fileinfo {
    float: left;
    width: 100%;
    background-color: rgba(255,255,255,1);
    transition: background-color 1s;
}

.uploadinfo .fileinfo:hover {
    background-color: rgba( 0,191,255,0.2);
}


.uploadinfo .progress {
    height: 4px;
    line-height: 4px;
    *zoom: 1;
    background: #fff;
    float: left;
    width: calc(100%);
    border: 1px #ccc solid;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    margin-bottom:7px;
}

.uploadinfo .filename {
    float: left;
    height: 20px;
    text-align: left;
    line-height: 20px;
    color: #333;
    overflow: hidden;
    width: calc(100% - 60%);
}


.uploadinfo .failed {
    font-size: 12px;
    color: #c50707;
    overflow: hidden;
    position:relative;
    top:-4px;
}

.uploadinfo .uploadstate {
    float: right;
    padding-top: 5px;
    height: 15px;
    text-align: right;
    font-size: 14px;
    line-height: 15px;
    color: #333;
    width: calc(100% - 40%);
}

.anim-rm-enter-active {
    animation: anim-in .5s;
}
.anim-rm-leave {
    transform: scale(0);
}
.anim-up-enter-active {
    animation: anim-in .5s;
}

.anim-up-leave {
    transform: scale(0);
}

@keyframes anim-in {
    0% {
        transform: scale(0);
    }
    50% {
        transform: scale(1.5);
    }
    100% {
        transform: scale(1);
    }
}

</style>
