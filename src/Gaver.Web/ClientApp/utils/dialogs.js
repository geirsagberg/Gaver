import bootbox from 'bootbox'

export function showPrompt (options) {
  const { message, placeholder } = options
  return new Promise((resolve, reject) => {
    bootbox.prompt(message, resolve)
  })
}
