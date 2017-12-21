import Vue from 'vue'
import router from './router'
import App from './app.vue'
import 'bootstrap'
import setupProgress from 'utils/progress'
import 'bootswatch/darkly/bootstrap.css'
import 'toastr/build/toastr.css'
import 'nprogress/nprogress.css'
import './css/site.css'

setupProgress()

Vue.config.productionTip = false

const vm = new Vue({
  el: '#app',
  router,
  render: (x) => x(App)
})
