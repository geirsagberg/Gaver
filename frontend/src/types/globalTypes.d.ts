declare interface Dictionary<T> {
  [index: string]: T
}

declare type List<T> = ArrayLike<T>

declare type Collection<T> = Dictionary<T> | List<T>
