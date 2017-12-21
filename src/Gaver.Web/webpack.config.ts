import { Configuration } from 'webpack'

const path = require('path')
const webpack = require('webpack')
const isDevelopment = process.env.ASPNETCORE_ENVIRONMENT === 'Development'
const ExtractTextPlugin = require('extract-text-webpack-plugin')
const UglifyJsPlugin = require('uglifyjs-webpack-plugin')
const ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin')

module.exports = {
  resolve: {
    modules: [ 'ClientApp', 'node_modules' ],
    extensions: [ '.js', '.jsx', '.ts', '.tsx' ]
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
        test: /\.(j|t)s(x?)$/,
        include: /ClientApp/,
	loader: 'ts-loader',
        options: {
	  transpileOnly: true
        }
      },
      {
        test: /\.(png|woff|woff2|eot|ttf|svg)$/,
        loader: 'url-loader',
        options: {
          limit: 100000
        }
      },
      {
	test: /\.vue$/,
	loader: 'vue-loader'
      },
      {
        test: /\.css$/,
        use: isDevelopment
          ? [ 'style-loader', 'css-loader?importLoaders=1', 'postcss-loader' ]
          : ExtractTextPlugin.extract({
            fallback: 'style-loader',
            use: [ 'css-loader?importLoaders=1', 'postcss-loader' ]
          })
      }
    ]
  },
  entry: {
    main: './ClientApp/app'
  },
  output: {
    path: path.resolve(__dirname, 'wwwroot', 'dist'),
    filename: '[name].js',
    publicPath: '/dist/'
  },
  plugins: [
    new ForkTsCheckerWebpackPlugin(),
    new webpack.DefinePlugin({
      'process.env.NODE_ENV': JSON.stringify(isDevelopment ? 'development' : 'production')
    }),
    new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery' }) // Maps these identifiers to the jQuery package (because Bootstrap expects it to be a global variable)
  ].concat(
    isDevelopment
      ? []
      : [
        new ExtractTextPlugin('styles.css'),
        new UglifyJsPlugin({
          sourceMap: true,
          uglifyOptions: { ecma: 8 }
        })
      ]
  ),
  devtool: 'source-map' // isDevelopment ? 'cheap-module-eval-source-map' : 'source-map'
} as Configuration
