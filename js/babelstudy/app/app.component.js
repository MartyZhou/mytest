import { Component } from '@angular/core';

@Component({
    selector: 'my-app',
    template: `
    <h1>{{title}}</h1>
    <input [(ngModel)]="title" placeholder="name"/>
    <my-heroes></my-heroes>
  `
})

export class AppComponent {
    constructor() {
        this.title = 'Tour of Heroes';
    }
}
