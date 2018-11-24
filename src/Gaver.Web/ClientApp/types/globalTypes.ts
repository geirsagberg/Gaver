interface Dictionary<T> {
  [index: string]: T
}

type List<T> = ArrayLike<T>

type Collection<T> = Dictionary<T> | List<T>
