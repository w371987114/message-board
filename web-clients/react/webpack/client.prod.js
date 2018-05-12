const path = require("path");
const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const StatsWebpackPlugin = require("stats-webpack-plugin");

module.exports = env => {
  if (!env || !env.API_URL) {
    throw "Environment variable 'API_URL' is required";
  }

  return {
    mode: "production",
    entry: path.join(__dirname, "../src/client.js"),
    output: {
      filename: "bundle.js",
      path: path.resolve(__dirname, "../dist/static"),
      publicPath: "/",
    },
    devtool: "source-map",
    module: {
      rules: [
        {
          enforce: "pre",
          test: /\.(js|jsx)$/,
          exclude: /node_modules/,
          loader: "eslint-loader",
        },
        {
          test: /\.(js|jsx)$/,
          exclude: /node_modules/,
          loader: "babel-loader",
        },
        {
          test: /\.css$/,
          use: [
            MiniCssExtractPlugin.loader,
            {
              loader: "css-loader",
              options: { minimize: true },
            },
          ],
        },
      ],
    },
    plugins: [
      new webpack.DefinePlugin({
        __API_URL__: JSON.stringify(env.API_URL),
      }),
      new MiniCssExtractPlugin({
        filename: "[name].css",
        chunkFilename: "[id].css",
      }),
      new StatsWebpackPlugin("../stats.json"),
    ],
  };
};