var webpack = require('webpack')
var ExtractTextPlugin = require('extract-text-webpack-plugin')

module.exports = {
  plugins: [
    new webpack.optimize.OccurenceOrderPlugin(),
    new webpack.optimize.UglifyJsPlugin({ compress: { warnings: false } }),
    new webpack.DefinePlugin({ 'process.env.NODE_ENV': '"production"' })
  ],
  module: {
    loaders: [
      { test: /\.css/, loader: ExtractTextPlugin.extract('site.css') }
    ]
  }
}
