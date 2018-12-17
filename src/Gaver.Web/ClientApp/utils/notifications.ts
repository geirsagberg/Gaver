import toastr from 'toastr'
import { isDevelopment } from '~/utils'
import { escape } from 'lodash-es'
import 'toastr/build/toastr.css'
import './notifications.css'

toastr.options.positionClass = 'toast-bottom-full-width'

export function showSuccess(message) {
  toastr.success(escape(message))
}

export function showError(message) {
  if (isDevelopment) {
    console.error(message)
  }
  toastr.error(escape(message))
}
