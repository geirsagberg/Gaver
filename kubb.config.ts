import { defineConfig } from '@kubb/core'
import { pluginOas } from '@kubb/plugin-oas'
import { pluginReactQuery } from '@kubb/plugin-react-query'
import { pluginTs } from '@kubb/plugin-ts'
import { pluginZod } from '@kubb/plugin-zod'

export default defineConfig({
  root: '.',
  input: {
    path: 'https://localhost:5001/swagger/v1/swagger.json',
  },
  output: {
    path: './frontend/src/api/generated',
  },
  plugins: [pluginOas({}), pluginTs({}), pluginZod({}), pluginReactQuery({})],
})
