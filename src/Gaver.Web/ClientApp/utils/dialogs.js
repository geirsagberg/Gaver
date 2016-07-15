// var vex = require('imports?exports=>false!vex-js')
import vex from 'vex-js'
import dialog from 'vex-js/js/vex.dialog'
if (BROWSER) {
  require('vex-js/css/vex.css')
  require('vex-js/css/vex-theme-top.css')
}

vex.defaultOptions.className = 'vex-theme-top'

export function showPrompt (options) {
  const { message, placeholder } = options
  return new Promise((resolve, reject) => {
    dialog.prompt({
      message,
      placeholder,
      callback: resolve
    })
  })
}
