import toastr from 'toastr'
import { isDevelopment } from 'utils'

toastr.options.positionClass = 'toast-bottom-full-width'

export function showSuccess (message) {
  toastr.success(message)
}

export function showError (message) {
  if (isDevelopment) {
    console.error(message)
  }
  toastr.error(message)
}