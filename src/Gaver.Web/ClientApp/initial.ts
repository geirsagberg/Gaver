import Trianglify from 'trianglify'

const background = document.getElementById('background')

generateBackground()

window.addEventListener('resize', generateBackground)

function generateBackground() {
  const pattern = Trianglify({
    width: window.innerWidth,
    height: window.innerHeight
  })
  const canvas = pattern.canvas() as HTMLCanvasElement
  if (background.childNodes[0]) {
    background.replaceChild(canvas, background.childNodes[0])
  } else {
    background.appendChild(canvas)
  }
}
