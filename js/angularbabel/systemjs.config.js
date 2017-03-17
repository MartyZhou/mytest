System.config({
  baseURL: "/",
  // defaultJSExtensions: true,
  transpiler: "babel",
  babelOptions: {
    "optional": [
      "runtime",
      "optimisation.modules.system",
      "es7.decorators"
    ]
  },
  paths: {
    "npm:": "node_modules/"
  },

  map: {
    "@angular/common": "npm:@angular/common@2.4.9",
    "@angular/core": "npm:@angular/core@2.4.9",
    "@angular/forms": "npm:@angular/forms@2.4.9",
    "@angular/platform-browser": "npm:@angular/platform-browser@2.4.9",
    "@angular/platform-browser-dynamic": "npm:@angular/platform-browser-dynamic@2.4.9",
    "@angular/router": "npm:@angular/router@3.4.9",
    "babel": "npm:babel-core/index.js",
    "core-js": "npm:core-js@1.2.7",
    "reflect-metadata": "npm:reflect-metadata@0.1.10",
    "rxjs": "npm:rxjs@5.2.0",
    "zone.js": "npm:zone.js@0.7.8"
  }
});
