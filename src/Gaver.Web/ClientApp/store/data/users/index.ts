import produce from 'immer'
import { UserModel } from '~/types/data'

export type UserData = Dictionary<UserModel>

const initialState: UserData = {}

export const reducer = produce((draft: UserData, action) => {}, initialState)
