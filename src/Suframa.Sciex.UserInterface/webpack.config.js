const HtmlWebpackPlugin = require('html-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const AotPlugin = require('@ngtools/webpack').AotPlugin;
const webpack = require('webpack');

module.exports = function () {
	return {
		entry: './www/main.ts',
		output: {
			path: __dirname + '/www/app',
			filename: 'app.js'
		},
		module: {
			rules: [
				{ test: /\.txt$/, use: 'raw-loader' }
			]
		},
		plugins: [			
			new CopyWebpackPlugin([
				{ from: 'www/assets', to: 'assets' }
			]),
			new HtmlWebpackPlugin({
				template: __dirname + '/www/index.html',
				output: __dirname + '/www/app',
				inject: 'head'
			})
		]
	};
}
