import { useEffect, useState } from 'react'
import { FeatureFlags } from '~/types/data'
import { tryOrNotify } from '.'
import { getJson } from './ajax'

export interface IAppSettings {
  audience: string
  clientId: string
  domain: string
}

let settings: Promise<IAppSettings> | undefined

export async function loadSettings() {
  const success = await tryOrNotify(
    () => settings ?? (settings = getJson('/api/auth'))
  )
  if (!success) {
    settings = undefined
  }
  return await settings
}

export const useFeatures = () => {
  const [features, setFeatures] = useState<FeatureFlags>()
  useEffect(() => {
    const fetchFeatures = () =>
      tryOrNotify(async () => {
        const features = await getJson<FeatureFlags>('/api/features')
        setFeatures(features)
      })
    fetchFeatures()
  }, [])
  return features
}
