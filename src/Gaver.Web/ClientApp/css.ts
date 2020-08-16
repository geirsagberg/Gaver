import { createCss, TCss, IConfig } from '@stitches/css'

const createCssConfig = <T extends IConfig>(config: T) => config

const cssConfig = createCssConfig({})

const stitchesCss = createCss(cssConfig)

// @stitches/css actually returns an object but says it is a string. This works fine in runtime for React, because it will call .toString() which actually gives the className. But some components, like react-tooltip, complain if we pass an object instead of a string.
// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-ignore
export const css: TCss<typeof cssConfig> = (...args) => stitchesCss(...args).toString()
