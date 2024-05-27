const path = require('path')

function resolve(dir) {
    return path.join(__dirname, '/', dir)
}


module.exports = {

    configureWebpack: {
        devtool: 'source-map'
    },
    chainWebpack: (config) => {
        config.resolve.alias
            .set('@$', resolve('src'))

        const svgRule = config.module.rule('svg')
        svgRule.uses.clear()
        svgRule
            .oneOf('inline')
            .resourceQuery(/inline/)
            .use('vue-svg-icon-loader')
            .loader('vue-svg-icon-loader')
            .end()
            .end()
            .oneOf('external')
            .use('file-loader')
            .loader('file-loader')
            .options({
                name: 'assets/[name].[hash:8].[ext]'
            })
    },

    productionSourceMap: false,
    lintOnSave: process.env.NODE_ENV !== 'production',

    css: {
        // 定位css文件
        sourceMap: true,
        loaderOptions: {
            less: {
                modifyVars: {
                    // less vars，customize ant design theme

                    // 默认蓝
                    'primary-color': '#1890ff',
                    'link-color': '#1890ff',

                    'font-size-base': '13px',
                    'font-size-middle': '15px',
                    'border-radius-base': '2px'
                },
                // DO NOT REMOVE THIS LINE
                javascriptEnabled: true
            }
        }
    },

    pluginOptions: {
        "style-resources-loader": {
            preProcessor: "less",
            patterns: [
                // 全局变量路径
                path.resolve(__dirname, "./src/assets/css/global.less"),
            ],
        }
    },

    devServer: {
        port: 8888, // 端口
        overlay: {
            warnings: true,
            errors: false
        }
    }
}