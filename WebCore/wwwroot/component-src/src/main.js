import Vue from 'vue/dist/vue.js'
import App from './App.vue'

new Vue({
    el: '#app',
    components: { App },
    template: "<App/>"
})
//new Vue({
//    render: h => h(App)
//}).$mount('#app')