import { Dialog, withMobileDialog } from '@material-ui/core'
import { DialogProps } from '@material-ui/core/Dialog'
import React, { FC } from 'react'

const InnerResponsiveDialog: FC<DialogProps> = props => <Dialog {...props} />
export const ResponsiveDialog = withMobileDialog<DialogProps>()(InnerResponsiveDialog)
