var path = require('path')
var webpack = require('webpack')
var ExtractTextPlugin = require('extract-text-webpack-plugin')
var merge = require('extendify')({ isDeep: true, arrays: 'concat' })
var devConfig = require('./webpack.config.dev')
var prodConfig = require('./webpack.config.prod')
var isDevelopment = process.env.ASPNETCORE_ENVIRONMENT === 'Development'

module.exports = merge({
  resolve: {
    root: path.join(__dirname, 'ClientApp'),
    extensions: [ '', '.js', '.jsx', '.ts', '.tsx' ]
  },
  module: {
    loaders: [
      { test: /\.js(x?)$/, include: /ClientApp/, loader: 'babel-loader' },
      { test: /\.(png|woff|woff2|eot|ttf|svg)$/, loader: 'url-loader?limit=100000' },
      { test: /\.css$/, loader: ExtractTextPlugin.extract('style-loader', 'css-loader?sourceMap') }
    ]
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
    new ExtractTextPlugin('site.css'),
    new webpack.ProvidePlugin({ 'window.jQuery': 'jquery', $: 'jquery', jQuery: 'jquery' }), // Maps these identifiers to the jQuery package (because Bootstrap expects it to be a global variable)
  // new webpack.DllReferencePlugin({
  //   context: __dirname,
  //   manifest: require('./wwwroot/dist/vendor-manifest.json')
  // }),
  ]
}, isDevelopment ? devConfig : prodConfig)
