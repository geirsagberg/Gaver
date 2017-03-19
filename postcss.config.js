module.exports = {
  plugins: [
    require('postcss-smart-import'),
    require('postcss-nested'),
    require('postcss-simple-vars'),
    require('postcss-custom-properties'),
    require('autoprefixer')
  ]
}