import { Configuration } from 'webpack'

const path = require('path')
const webpack = require('webpack')

const isDevelopment = process.env.ASPNETCORE_ENVIRONMENT === 'Development'

const mode = isDevelopment ? 'development' : 'production'
const ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin')
const MiniCssExtractPlugin = require('mini-css-extract-plugin')

const baseDir = process.cwd()

const productionPlugins = [
  new ForkTsCheckerWebpackPlugin({
    checkSyntacticErrors: true,
    silent: false,
    tsconfig: path.join(__dirname, 'tsconfig.json')
  }),
  new MiniCssExtractPlugin({
    filename: '[name].css'
  })
]

const developmentPlugins = []

const configuration: Configuration = {
  mode,
  devtool: isDevelopment ? 'source-map' : 'nosources-source-map',
  resolve: {
    alias: {
      '~': path.join(baseDir, 'ClientApp'),
      lodash: 'lodash-es'
    },
    extensions: ['.js', '.jsx', '.ts', '.tsx']
  },
  module: {
    rules: [
      {
        test: /\.(t|j)sx?$/,
        use: [
          {
            loader: 'thread-loader',
            options: {
              // there should be 1 cpu for the fork-ts-checker-webpack-plugin in prod
              workers: require('os').cpus().length - (isDevelopment ? 0 : 1)
            }
          },
          {
            loader: 'babel-loader',
            options: {
              extends: path.join(__dirname, 'babel.config.js'),
              cacheDirectory: true
            }
          }
        ]
      },
      {
        test: /\.(png|jpg|gif|woff|woff2)$/,
        use: [
          {
            loader: 'url-loader',
            options: {
              limit: 4096
            }
          }
        ]
      },
      {
        test: /\.css$/,
        use: [isDevelopment ? 'style-loader' : MiniCssExtractPlugin.loader, 'css-loader', 'postcss-loader']
      }
    ]
  },
  entry: {
    main: './ClientApp/app'
  },
  output: {
    path: path.join(baseDir, 'wwwroot', 'dist'),
    filename: '[name].js',
    publicPath: '/dist/'
  },
  optimization: {
    splitChunks: {
      chunks: 'all'
    }
  },
  plugins: [
    new webpack.DefinePlugin({
      'process.env.NODE_ENV': `'${mode}'`
    })
  ].concat(isDevelopment ? developmentPlugins : productionPlugins)
}

module.exports = configuration
