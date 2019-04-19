import cgUpload from './cgUpload.vue';

/* istanbul ignore next */
cgUpload.install = function (Vue) {
  Vue.component(cgUpload.name, cgUpload);
};


/* istanbul ignore if */
if (typeof window !== 'undefined' && window.Vue) {
  cgUpload.install(window.Vue);
}
export default cgUpload;
