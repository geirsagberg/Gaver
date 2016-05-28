const path = require('path')

module.exports = {
  context: path.resolve('app'),
  entry: './index.js',
  output: {
    path: path.resolve('Gaver.Web/wwwroot/dist'),
    filename: 'bundle.js'
  },
  devtool: 'cheap-module-eval-source-map',
  module: {
    loaders: [
      {
        test: /\.jsx?$/,
        exclude: /node_modules/,
        loader: 'babel',
        query: {
          presets: [
            'es2015', 'react'
          ]
        }
      }
    ]
  }
}