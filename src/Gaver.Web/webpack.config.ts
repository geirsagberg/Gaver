import { Configuration } from 'webpack'

const path = require('path')
const webpack = require('webpack')
const isDevelopment = process.env.ASPNETCORE_ENVIRONMENT === 'Development'
const ExtractTextPlugin = require('mini-css-extract-plugin')
const HardSourceWebpackPlugin = require('hard-source-webpack-plugin')

const configuration: Configuration = {
  mode: isDevelopment ? 'development' : 'production',
  resolve: {
    alias: {
      '~': path.join(__dirname, 'src', 'Gaver.Web', 'ClientApp'),
      lodash: 'lodash-es'
    },
    extensions: ['.js', '.jsx', '.ts', '.tsx']
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
        loader: 'awesome-typescript-loader',
        options: {
          silent: true,
          useCache: true
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
        test: /\.css$/,
        use: [isDevelopment ? 'style-loader' : ExtractTextPlugin.loader, 'css-loader', 'postcss-loader']
      }
    ]
  },
  entry: {
    main: './ClientApp/boot-client'
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
    new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery' }) // Maps these identifiers to the jQuery package (because Bootstrap expects it to be a global variable)
  ].concat(isDevelopment ? [new HardSourceWebpackPlugin()] : [new ExtractTextPlugin({ filename: 'styles.css' })]),
  devtool: isDevelopment ? 'cheap-module-eval-source-map' : 'nosources-source-map'
}

module.exports = configuration
