import { clone } from 'lodash-es'
import { AddWishRequest, DeleteWishResponse, MyListDto, UpdateWishRequest } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { deleteJson, getJson, patchJson, postJson } from '~/utils/ajax'
import { normalizeArrays } from '~/utils/normalize'
import { showError, showSuccess } from '~/utils/notifications'
import { Context } from '..'
import { getEmptyWish, Wish, MyListState } from './state'

export const handleMyList = async ({ actions }: Context) => {
  actions.routing.setCurrentPage('myList')
  await actions.myList.loadWishes()
}

export const addWish = ({ state: { myList } }: Context) =>
  tryOrNotify(async () => {
    if (myList.newWish) {
      const { title, url } = myList.newWish
      const addWishRequest: AddWishRequest = {
        title,
        url,
      }
      const wish = await postJson('/api/MyList', addWishRequest)
      myList.wishes[wish.id] = wish
      delete myList.newWish
      myList.wishesOrder.push(wish.id)
    }
  })

export const startAddingWish = ({ state: { myList } }: Context) => {
  myList.newWish = getEmptyWish()
}

export const cancelAddingWish = ({ state: { myList } }: Context) => {
  delete myList.newWish
}

export const startEditingWish = ({ state: { myList } }: Context, wishId: number) => {
  const wish = myList.wishes[wishId]
  myList.editingWish = { ...wish }
}

export const deleteEditingWish = async ({ state: { myList }, actions }: Context, wishId: number) => {
  await actions.myList.confirmDeleteWish(wishId)
  delete myList.editingWish
}

export const confirmDeleteWish = ({ state: { myList } }: Context, wishId: number) =>
  tryOrNotify(async () => {
    const response = await deleteJson<DeleteWishResponse>(`/api/MyList/${wishId}`)
    myList.wishesOrder = response.wishesOrder
    delete myList.wishes[wishId]
  })

export const cancelEditingWish = ({ state: { myList } }: Context) => {
  delete myList.editingWish
}

export const updateEditingWish = ({ state: { myList } }: Context, update: Partial<Wish>) => {
  myList.editingWish = {
    ...myList.editingWish,
    ...(update as Wish),
  }
}

export const saveEditingWish = ({ state: { myList } }: Context) =>
  tryOrNotify(async () => {
    const wish = myList.editingWish
    if (wish?.id) {
      await patchJson<UpdateWishRequest>(`/api/MyList/${wish.id}`, wish)
      myList.wishes[wish.id] = clone(wish)
      myList.editingWish = undefined
    }
  })

export const updateNewWish = ({ state }: Context, update: Partial<Wish>) => {
  state.myList.newWish = {
    ...state.myList.newWish,
    ...(update as Wish),
  }
}

export const loadWishes = ({ state }: Context) =>
  tryOrNotify(async () => {
    const model = await getJson<MyListDto>('/api/MyList')
    const { myList } = state
    myList.wishes = normalizeArrays(model.wishes)
    myList.id = model.id
    myList.wishesOrder = model.wishesOrder
    myList.wishesLoaded = true
  })

export const startSharingList = ({ state: { myList } }: Context) => {
  myList.isSharingList = true
  myList.shareUrl = undefined // Reset any previous share URL
}

export const toggleDeleting = ({ state: { myList } }: Context) => {
  myList.isDeleting = !myList.isDeleting
}

export const cancelSharingList = ({ state: { myList } }: Context) => {
  myList.isSharingList = false
  myList.shareUrl = undefined
}

export const createShareLink = async ({ state: { myList } }: Context) => {
  if (!myList.id) {
    showError('Ønskeliste ikke lastet')
    return
  }

  try {
    // Call backend to create invitation token and get share URL
    const response = await postJson<{ shareUrl: string }>('/api/MyList/Share', {})
    myList.shareUrl = response.shareUrl
  } catch (error) {
    console.error('Failed to create share link:', error)
    showError('Kunne ikke opprette delingslenke')
  }
}

export const copyShareLink = ({ state: { myList } }: Context) => {
  const shareUrl = myList.shareUrl
  if (!shareUrl) {
    showError('Ingen lenke å kopiere')
    return
  }

  try {
    // Try modern clipboard API first
    if (navigator.clipboard && navigator.clipboard.writeText) {
      // Call clipboard API but don't await - keeps function synchronous for Safari.
      // Safari requires clipboard operations to be in direct response to user interaction.
      navigator.clipboard.writeText(shareUrl).then(
        () => {
          showSuccess('Delingslenke kopiert!')
          myList.isSharingList = false
          myList.shareUrl = undefined
        },
        (clipboardError) => {
          // Clipboard API failed, try fallback
          console.warn('Clipboard API failed, using fallback:', clipboardError)
          copyWithFallback(shareUrl, myList)
        }
      )
    } else {
      // Use fallback for older browsers
      copyWithFallback(shareUrl, myList)
    }
  } catch (error) {
    console.error('Copy to clipboard failed:', error)
    showError('Kunne ikke kopiere lenke')
  }
}

function copyWithFallback(shareUrl: string, myList: MyListState) {
  const textArea = document.createElement('textarea')
  textArea.value = shareUrl
  textArea.style.position = 'fixed'
  textArea.style.left = '-999999px'
  textArea.style.top = '-999999px'
  document.body.appendChild(textArea)
  textArea.focus()
  textArea.select()
  try {
    const successful = document.execCommand('copy')
    if (successful) {
      showSuccess('Delingslenke kopiert!')
      myList.isSharingList = false
      myList.shareUrl = undefined
    } else {
      throw new Error('execCommand copy failed')
    }
  } catch (error) {
    console.error('Fallback copy failed:', error)
    showError('Kunne ikke kopiere lenke')
  } finally {
    document.body.removeChild(textArea)
  }
}

interface WishOrderChangedParams {
  oldIndex: number
  newIndex: number
  wishId: number
}
export const wishOrderChanged = async ({ state: { myList } }: Context, payload: WishOrderChangedParams) => {
  if (payload.newIndex === payload.oldIndex) {
    return
  }
  const originalOrder = myList.wishesOrder.slice()
  myList.wishesOrder.splice(payload.oldIndex, 1)
  myList.wishesOrder.splice(payload.newIndex, 0, payload.wishId)

  try {
    await postJson('/api/MyList/Order', { wishesOrder: myList.wishesOrder })
  } catch (error) {
    myList.wishesOrder = originalOrder
    showError(error as Error)
  }
}
