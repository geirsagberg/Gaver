module.exports = api => {
  const isTest = api.env('test')
  return {
    presets: [['@babel/env', { modules: isTest ? 'auto' : false }], '@babel/react', '@babel/typescript'],
    plugins: [
      '@babel/plugin-proposal-export-default-from',
      '@babel/plugin-proposal-object-rest-spread',
      '@babel/plugin-proposal-optional-chaining',
      ['@babel/plugin-proposal-class-properties', { loose: true }],
      'react-hot-loader/babel'
    ]
  }
}
