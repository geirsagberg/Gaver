export const scrollChat = () =>
  setTimeout(() => {
    const simpleBar: SimpleBar = (
      document.getElementById('chatMessages') as any
    )['SimpleBar']
    if (simpleBar) {
      const scrollEl = simpleBar.getScrollElement()
      scrollEl.scrollTo({ top: scrollEl.scrollHeight, behavior: 'smooth' })
    }
  })
