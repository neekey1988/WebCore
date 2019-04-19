var __httpRequest={
    uploadProgress: function (e,evt) { },
    uploadComplete: function () { },
    uploadError: function (e,ev) { },
    uploadCanceled: function () { },
    uploadTimeOut: function (e,ev) { },
    submit: function (f,url, _params) {
        var self = this;
        var xhr = new XMLHttpRequest();
        _params = _params.split("&");
        xhr.upload.onprogress = function (evt) {
            self.uploadProgress(f, evt);
        };
        xhr.ontimeout = function (ev) {
            self.uploadTimeOut(f, ev);
        };
        xhr.onload = function (ev) {
            self.uploadComplete(f, ev);
        };
        xhr.onerror = function (ev) {
            self.uploadError(f, ev);
        };
        var _fd = new FormData();
        _fd.append("file", f.file, f.file.name);
        for (var k = 0; k < _params.length; k++) {
            _fd.append(_params[k].split("=")[0], _params[k].split("=")[1]);
        }
        try {
            xhr.open("post", url);
            xhr.responseType = "json";
            xhr.timeout = 60*1000; //ms
            xhr.send(_fd);
        } catch (e) {
            if (xhr != null && xhr != undefined) {
                xhr.abort();
            }
            return null;
        }
        return xhr;
    }
};
export default __httpRequest;




//var self = this;
//if (this.files.length == 0) {
//    return;
//}
//var xhr = new XMLHttpRequest();
//var _params = this.params.split("&");
//try {
//    for (var i = 0; i < this.files.length; i++) {
//        if (md5 != undefined && md5 != "" && this.files[i].md5 != md5)
//            continue;
//        if (this.files[i].status != "ready")
//            continue;
//        xhr = new XMLHttpRequest();
//        var tempfile = this.files[i].file;
//        (function (_i) {
//            xhr.upload.onprogress = function (evt) {
//                self.uploadProgress(self.files[_i], evt);
//            };
//            xhr.ontimeout = function (ev) {
//                self.uploadTimeOut(self.files[_i], ev);
//            };
//            xhr.onload = function (ev) {
//                self.uploadComplete(self.files[_i], ev);
//            };
//            xhr.onerror = function (ev) {
//                self.uploadError(self.files[_i], ev);
//            };
//        })(i);

//        var _fd = new FormData();
//        _fd.append("file", tempfile, tempfile.name);
//        for (var k = 0; k < _params.length; k++) {
//            _fd.append(_params[k].split("=")[0], _params[k].split("=")[1]);
//        }
//        xhr.open("post", this.url);
//        xhr.responseType = "json";
//        xhr.send(_fd);
//    }
//} catch (e) {
//    //终止上传
//    if (xhr != null && xhr != undefined) {
//        xhr.abort();
//    }
//}