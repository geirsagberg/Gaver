import { RootState } from '..'

export const selectIsSavingOrLoading = (state: RootState) => state.app.isSavingOrLoading
