import { useEffect } from 'react'
import { useActions } from '~/overmind'
import { NavContext } from '~/overmind/app'

export function useNavContext(navContext: NavContext, deps?: readonly any[]) {
  const {
    app: { setNavContext },
  } = useActions()
  useEffect(() => {
    setNavContext(navContext)
  }, deps)
}
