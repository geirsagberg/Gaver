import React, { FC } from 'react'
import ErrorView from '~/components/ErrorView'

const NotFoundPage: FC = () => {
  return <ErrorView>Det ser ut til at siden du forsøkte å gå til, ikke finnes.</ErrorView>
}

export default NotFoundPage
