System.config({
    baseURL: '/',
    defaultJSExtensions: true,
    transpiler: 'babel',
    babelOptions: {
        optional: [
            'runtime',
            'optimisation.modules.system',
            'es7.decorators'
        ]
    },
    paths: {
        'npm:*': 'node_modules/*'
    },
    map: {
        'babel': 'npm:babel-core'
    }
})