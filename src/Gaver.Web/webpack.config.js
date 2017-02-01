var path = require('path')
var webpack = require('webpack')
var isDevelopment = process.env.ASPNETCORE_ENVIRONMENT === 'Development'
var ExtractTextPlugin = require('extract-text-webpack-plugin')

module.exports = {
  resolve: {
    modules: [
      'ClientApp',
      'node_modules'
    ],
    extensions: ['.js', '.jsx']
  },
  module: {
    rules: [
      {
        test: /jquery\.js$/,
        use: [
          {
            loader: 'expose-loader',
            options: 'jQuery'
          },
          {
            loader: 'expose-loader',
            options: '$'
          }
        ]
      },
      {
        test: /\.js(x?)$/, include: /ClientApp/, loader: 'babel-loader'
      },
      {
        test: /\.(png|woff|woff2|eot|ttf|svg)$/,
        loader: 'url-loader',
        options: {
          limit: 100000
        }
      },
      {
        test: /\.css$/,
        use: isDevelopment
          ? [
            'style-loader',
            'css-loader?importLoaders=1',
            'postcss-loader'
          ]
          : ExtractTextPlugin.extract({
            fallbackLoader: 'style-loader',
            loader: [
              'css-loader?importLoaders=1',
              'postcss-loader'
            ]
          })
      },
    ],
  },
  entry: {
    main: './ClientApp/boot-client.jsx'
  },
  output: {
    path: path.resolve(__dirname, 'wwwroot', 'dist'),
    filename: '[name].js',
    publicPath: '/dist/'
  },
  plugins: [
    new webpack.DefinePlugin({
      'process.env.NODE_ENV': JSON.stringify(isDevelopment ? 'development' : 'production')
    }),
    new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery' }), // Maps these identifiers to the jQuery package (because Bootstrap expects it to be a global variable)
  ].concat(isDevelopment
    ? []
    : [
      new ExtractTextPlugin('styles.css'),
      new webpack.optimize.UglifyJsPlugin({
        sourceMap: true
      })
    ]),
  devtool: isDevelopment ? 'source-map' : 'cheap-module-source-map',
  node: {
    console: true,
    fs: 'empty',
    net: 'empty',
    tls: 'empty'
  }
}
