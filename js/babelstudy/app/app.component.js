import { Component } from '@angular/core';

@Component({
    selector: 'my-app',
    template: `<h1>Hello {{name}}</h1>`
})

export class AppComponent { 
    constructor() {
        this.name = 'Angular 2';
    }
}