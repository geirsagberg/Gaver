import bootbox from 'bootbox'
import Promise from 'bluebird'

export function showPrompt (options) {
  const { title, message, placeholder, value } = options
  return new Promise((resolve, reject) => {
    bootbox.prompt({
      title,
      message,
      callback: resolve,
      value,
      backdrop: true
    })
  })
}

export function showConfirm (message) {
  return new Promise((resolve, reject) => {
    bootbox.confirm({
      message,
      callback: resolve,
      backdrop: true
    })
  })
}