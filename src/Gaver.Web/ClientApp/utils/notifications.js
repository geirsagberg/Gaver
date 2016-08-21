import toastr from 'toastr'

toastr.options.positionClass = 'toast-bottom-full-width'

export function showSuccess (message) {
  toastr.success(message)
}

export function showError (message) {
  toastr.error(message)
}