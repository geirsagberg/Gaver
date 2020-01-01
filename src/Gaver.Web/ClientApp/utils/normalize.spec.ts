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
  it('turns a nested array into a dictionary using "id" as key', () => {
    const foo = {
      bar: 'x',
      items: [
        { id: 1, name: 'A' },
        { id: 2, name: 'B' }
      ]
    }

    const normalizedFoo = normalizeArrays(foo)

    expect(normalizedFoo).toEqual({
      bar: 'x',
      items: {
        '1': { id: 1, name: 'A' },
        '2': { id: 2, name: 'B' }
      }
    })
  })
  it('does not touch nulls', () => {
    const foo = [{ id: 1, name: null }]

    const normalizedFoo = normalizeArrays(foo)

    expect(normalizedFoo).toEqual({
      '1': { id: 1, name: null }
    })
  })
  it('does not touch primitive arrays', () => {
    const foo = [1, 2, 3]

    const normalizedFoo = normalizeArrays(foo)

    expect(normalizedFoo).toEqual([1, 2, 3])
  })
  it('does not touch nested primitive arrays', () => {
    const foo = {
      items: [1, 2, 3]
    }

    const normalizedFoo = normalizeArrays(foo)

    expect(normalizedFoo).toEqual(foo)
  })
  it('maps empty array to empty object', () => {
    const foo = []

    const normalizedFoo = normalizeArrays(foo)

    expect(normalizedFoo).toEqual({})
  })
  it('maps nested empty array to empty object', () => {
    const foo = {
      items: []
    }

    const normalizedFoo = normalizeArrays(foo)

    expect(normalizedFoo).toEqual({ items: {} })
  })
})
