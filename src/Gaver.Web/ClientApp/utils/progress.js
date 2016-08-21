import NProgress from 'nprogress'

export default function () {
  document.addEventListener('fetch', event => {
    console.log(event)
  })
}
