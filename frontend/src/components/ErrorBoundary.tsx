import React, { ReactNode } from 'react'
import ErrorView from './ErrorView'

class ErrorBoundary extends React.Component<{ children: ReactNode }, { hasError: boolean }> {
  state = {
    hasError: false,
  }
  static getDerivedStateFromError() {
    return { hasError: true }
  }
  render() {
    return this.state.hasError ? (
      <ErrorView onBackClicked={() => this.setState({ hasError: false })}>Noe gikk galt!</ErrorView>
    ) : (
      this.props.children
    )
  }
}

export default ErrorBoundary
