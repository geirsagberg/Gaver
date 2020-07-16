import { useEffect } from 'react'
import { useOvermind } from '~/overmind'
import { NavContext } from '~/overmind/app'

export function useNavContext(navContext: NavContext, deps?: readonly any[]) {
  const {
    actions: {
      app: { setNavContext },
    },
  } = useOvermind()
  useEffect(() => {
    setNavContext(navContext)
  }, deps)
}
