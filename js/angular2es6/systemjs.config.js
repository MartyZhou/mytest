System.config({
    baseURL: '/',
    transpiler: 'plugin-babel',
    babelOptions: {
        optional: [
            'runtime',
            'optimisation.modules.system',
            'es7.decorators'
        ]
    },
    paths: {
        'npm:': 'node_modules/'
    },
    map: {
        'plugin-babel': 'npm:systemjs-plugin-babel/plugin-babel.js',
        'systemjs-babel-build': 'npm:systemjs-plugin-babel/systemjs-babel-node.js',
        '@angular/platform-browser-dynamic': 'npm:@angular/platform-browser-dynamic/index.js'
    }
})