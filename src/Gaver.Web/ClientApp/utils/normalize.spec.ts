import { normalizeArrays } from './normalize'

describe('normalizeArrays', () => {
  it('turns a top-level array into a dictionary using "id" as key', () => {
    const foo = [
      { id: 1, name: 'A' },
      { id: 2, name: 'B' }
    ]

    const normalizedFoo = normalizeArrays(foo)

    expect(normalizedFoo).toEqual({
      '1': { id: 1, name: 'A' },
      '2': { id: 2, name: 'B' }
    })
  })
  it('does not touch nulls', () => {
    const foo = [{ id: 1, name: null }]

    const normalizedFoo = normalizeArrays(foo)
    expect(normalizedFoo).toEqual({
      '1': { id: 1, name: null }
    })
  })
})
