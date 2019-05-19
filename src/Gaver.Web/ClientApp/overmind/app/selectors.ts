import { ResolvedState } from '..'

export const selectIsSavingOrLoading = (state: ResolvedState) => state.app.isSavingOrLoading
