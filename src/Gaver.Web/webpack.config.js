var path = require('path')
var webpack = require('webpack')
var merge = require('extendify')({ isDeep: true, arrays: 'concat' })
var devConfig = require('./webpack.config.dev')
var prodConfig = require('./webpack.config.prod')
var isDevelopment = process.env.ASPNETCORE_ENVIRONMENT === 'Development'
var autoPrefixer = require('autoprefixer')
var precss = require('precss')
var ExtractTextPlugin = require('extract-text-webpack-plugin')

module.exports = merge({
  resolve: {
    root: path.join(__dirname, 'ClientApp'),
    extensions: ['', '.js', '.jsx', '.ts', '.tsx']
  },
  module: {
    loaders: [
      { test: /jquery\.js$/, loader: 'expose?jQuery!expose?$' },
      { test: /\.js(x?)$/, include: /ClientApp/, loader: 'babel' },
      { test: /\.(png|woff|woff2|eot|ttf|svg)$/, loader: 'url?limit=100000' },
      { test: /\.css$/, loader: isDevelopment ? 'style!css!postcss?sourceMap=inline' : ExtractTextPlugin.extract('style', 'css!postcss?sourceMap=inline') }
    ]
  },
  postcss: function () {
    return [autoPrefixer, precss]
  },
  entry: {
    main: ['./ClientApp/boot-client.jsx']
  },
  output: {
    path: path.join(__dirname, 'wwwroot', 'dist'),
    filename: '[name].js',
    publicPath: '/dist/'
  },
  plugins: [
    new webpack.DefinePlugin({
      'process.env.NODE_ENV': JSON.stringify(isDevelopment ? 'development' : 'production')
    }),
    new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery' }), // Maps these identifiers to the jQuery package (because Bootstrap expects it to be a global variable)
  ].concat(isDevelopment ? [] : [
    new ExtractTextPlugin('styles.css')
  ])
}, isDevelopment ? devConfig : prodConfig)
