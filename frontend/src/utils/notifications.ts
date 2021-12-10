import { escape } from 'lodash-es'
import toastr from 'toastr'
import 'toastr/build/toastr.css'
import { isDevelopment } from '~/utils'
import './notifications.css'

toastr.options.positionClass = 'toast-bottom-full-width'

export function showSuccess(message: string) {
  toastr.success(escape(message))
}

export function showError(message: Error | string) {
  if (isDevelopment) {
    console.error(message)
  }
  toastr.error(escape(message instanceof Error ? message.message : message))
}

export function showConfirm(message: string): Promise<boolean> {
  return new Promise((resolve) => resolve(confirm(message)))
}
