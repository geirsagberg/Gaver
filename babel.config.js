module.exports = {
  presets: [['@babel/env', { modules: false }], '@babel/react', '@babel/typescript'],
  plugins: [
    '@babel/plugin-proposal-export-default-from',
    '@babel/plugin-proposal-object-rest-spread',
    ['@babel/plugin-proposal-class-properties', { loose: true }],
    'react-hot-loader/babel'
  ]
}
